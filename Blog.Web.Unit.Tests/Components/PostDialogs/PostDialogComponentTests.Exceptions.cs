using System.Threading.Tasks;
using Blog.Web.Models.PostViews;
using Blog.Web.Views.Components.PostDialogs;
using FluentAssertions;
using Moq;
using Xeptions;
using Xunit;

namespace Blog.Web.Unit.Tests.Components.PostDialogs
{
    public partial class PostDialogComponentTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldRenderValidationDetailOnPostAsync(Xeption postViewValidationException)
        {
            // given
            string someContent = GetRandomContent();

            this.postViewServiceMock.Setup(service =>
                service.AddPostViewAsync(It.IsAny<PostView>()))
                    .ThrowsAsync(postViewValidationException);

            // when

            this.postDialogRenderedComponent =
                RenderComponent<PostDialog>();

            this.postDialogRenderedComponent.Instance
                .OpenDialog();

            await this.postDialogRenderedComponent.Instance.TextArea
                .SetValueAsync(someContent);

            this.postDialogRenderedComponent.Instance.Dialog
                .Click();

            // then
            this.postDialogRenderedComponent.Instance.Dialog.IsVisible
                .Should().BeTrue();

            this.postDialogRenderedComponent.Instance.TextArea.IsDisabled
                .Should().BeFalse();

            this.postDialogRenderedComponent.Instance.Dialog.DialogButton.Disabled
                .Should().BeFalse();

            this.postDialogRenderedComponent.Instance.Spinner.IsVisible
                .Should().BeFalse();

            this.postDialogRenderedComponent.Instance.ContentValidationSummary.ValidationData
                .Should().BeEquivalentTo(postViewValidationException.InnerException.Data);

            this.postDialogRenderedComponent.Instance.ContentValidationSummary.Color
                .Should().Be("Red");

            this.postViewServiceMock.Verify(service =>
                service.AddPostViewAsync(It.IsAny<PostView>()),
                    Times.Once);

            this.postViewServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldRenderDependencyErrorDetailsOnPostAsync(Xeption postViewDependencyException)
        {
            // given
            string someContent = GetRandomContent();

            this.postViewServiceMock.Setup(service =>
                service.AddPostViewAsync(It.IsAny<PostView>()))
                    .ThrowsAsync(postViewDependencyException);
            // when
            this.postDialogRenderedComponent =
                RenderComponent<PostDialog>();

            this.postDialogRenderedComponent.Instance
                .OpenDialog();

            await this.postDialogRenderedComponent.Instance.TextArea
                .SetValueAsync(someContent);

            this.postDialogRenderedComponent.Instance.Dialog
                .Click();

            // then
            this.postDialogRenderedComponent.Instance.Dialog.IsVisible
                .Should().BeTrue();

            this.postDialogRenderedComponent.Instance.TextArea.IsDisabled
                .Should().BeFalse();

            this.postDialogRenderedComponent.Instance.Dialog.DialogButton.Disabled
                .Should().BeFalse();

            this.postDialogRenderedComponent.Instance.Spinner.IsVisible
                .Should().BeFalse();

            this.postDialogRenderedComponent.Instance.ContentValidationSummary.Message
                .Should().Be(postViewDependencyException.Message);

            this.postDialogRenderedComponent.Instance.ContentValidationSummary.ValidationData
                .Count.Should().Be(0);

            this.postDialogRenderedComponent.Instance.ContentValidationSummary.Color
                .Should().Be("Red");

            this.postViewServiceMock.Verify(service =>
                service.AddPostViewAsync(It.IsAny<PostView>()),
                    Times.Once);

            this.postViewServiceMock.VerifyNoOtherCalls();
        }
    }
}
