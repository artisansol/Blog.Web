using System.Threading.Tasks;
using Blog.Web.Models.PostViews;
using Blog.Web.Models.Views.Components.PostDialogs;
using Blog.Web.Views.Components.PostDialogs;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Web.Unit.Tests.Components.PostDialogs
{
    public partial class PostDialogComponentTests
    {
        [Fact]
        public void ShouldInitializeComponent()
        {
            // given
            PostDialogComponentState expectedState =
                PostDialogComponentState.Loading;

            // when
            var initialPostDialog =
                new PostDialog();

            // then
            initialPostDialog.State.Should().Be(expectedState);
            initialPostDialog.PostViewService.Should().BeNull();
            initialPostDialog.PostView.Should().BeNull();
            initialPostDialog.Dialog.Should().BeNull();
            initialPostDialog.TextArea.Should().BeNull();
            initialPostDialog.IsVisible.Should().BeFalse();

        }

        [Fact] 
        public void ShouldDisplayDialogIfOpenDialogIsClicked()
        {
            // given
            string expectedTextAreaHeight = "250px";

            PostDialogComponentState expectedState = 
                PostDialogComponentState.Content;

            var expectedPostView = new PostView();

            // when
            this.postDialogRenderedComponent = RenderComponent<PostDialog>();
            this.postDialogRenderedComponent.Instance.OpenDialog();

            // then
            this.postDialogRenderedComponent.Instance.State.Should().Be(expectedState);

            this.postDialogRenderedComponent.Instance.PostViewService.Should().NotBeNull();

            this.postDialogRenderedComponent.Instance.PostView.Should().BeEquivalentTo(expectedPostView);

            this.postDialogRenderedComponent.Instance.Dialog.Should().NotBeNull();

            this.postDialogRenderedComponent.Instance.Dialog.IsVisible.Should().BeTrue();

            this.postDialogRenderedComponent.Instance.Dialog.ButtonTitle.Should().Be("Post");

            this.postDialogRenderedComponent.Instance.Dialog.Title.Should().Be("NEW POST");

            this.postDialogRenderedComponent.Instance.IsVisible.Should().BeTrue();

            this.postDialogRenderedComponent.Instance.TextArea.Should().NotBeNull();

            this.postDialogRenderedComponent.Instance.TextArea.Height.Should().Be(expectedTextAreaHeight);
        }

        [Fact]
        public async Task ShouldSubmitPostViewAsync()
        {
            // given
            string randomContent = GetRandomContent();
            string inputContent = randomContent;
            string expectedContent = inputContent;

            var expectedPostView = new PostView
            {
                Content = inputContent,
            };

            // when
            this.postDialogRenderedComponent = 
                RenderComponent<PostDialog>();

            this.postDialogRenderedComponent.Instance.OpenDialog();
            await this.postDialogRenderedComponent.Instance.TextArea.SetValueAsync(inputContent);
            this.postDialogRenderedComponent.Instance.Dialog.Click();

            // then
            this.postDialogRenderedComponent.Instance.Dialog.IsVisible.Should().BeFalse();
            this.postDialogRenderedComponent.Instance.PostView.Should().BeEquivalentTo(expectedPostView);

            this.postViewServiceMock.Verify(service => 
                service.AddPostViewAsync(this.postDialogRenderedComponent.Instance.PostView), 
                Times.Once);

            this.postViewServiceMock.VerifyNoOtherCalls();

        }

    }
}
