using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Views.Components.PostDialogs;
using Blog.Web.Views.Components.PostDialogs;
using FluentAssertions;
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
            initialPostDialog.Dialog.Should().BeNull();
            initialPostDialog.TextArea.Should().BeNull();
            initialPostDialog.IsVisible.Should().BeFalse();

        }

        [Fact]
        public void ShouldDisplayDialogIfOpenDialogIsClicked()
        {
            // given
            PostDialogComponentState expectedState = PostDialogComponentState.Content;

            // when
            this.postDialogRenderedComponent = RenderComponent<PostDialog>();
            this.postDialogRenderedComponent.Instance.OpenDialog();

            // then
            this.postDialogRenderedComponent.Instance.State
                .Should().Be(expectedState);

            this.postDialogRenderedComponent.Instance.PostViewService
                .Should().NotBeNull();

            this.postDialogRenderedComponent.Instance.Dialog
                .Should().NotBeNull();

            this.postDialogRenderedComponent.Instance.TextArea
                .Should().NotBeNull();

            this.postDialogRenderedComponent.Instance.Dialog.IsVisible
                .Should().BeTrue();

            this.postDialogRenderedComponent.Instance.Dialog.ButtonTitle
                .Should().Be("Post");

            this.postDialogRenderedComponent.Instance.Dialog.Title
                .Should().Be("New Post");

            this.postDialogRenderedComponent.Instance.IsVisible
                .Should().BeTrue();
        }
    }
}
