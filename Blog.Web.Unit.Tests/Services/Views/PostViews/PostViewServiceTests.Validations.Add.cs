using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using Blog.Web.Models.PostViews;
using Blog.Web.Models.PostViews.Exceptions;
using Moq;
using Xunit;

namespace Blog.Web.Unit.Tests.Services.Views.PostViews
{
    public partial class PostViewServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfPostViewIdIsNullAndLogItAsync()
        {
            // given
            PostView nullPostView = null;
            var nullPostViewException = new NullPostViewException();

            var expectedPostViewValidationException = 
                new PostViewValidationException(nullPostViewException);

            // when
            ValueTask<PostView> addPostViewTask = 
                this.postViewService.AddPostViewAsync(nullPostView);

            // then
            await Assert.ThrowsAsync<PostViewValidationException>(() => 
                addPostViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewValidationException))),
                    Times.Once);

            this.postServiceMock.Verify(service => 
                service.AddPostAsync(It.IsAny<Post>()), 
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnAddIfPostViewIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidPostView = new PostView
            {
                Title = invalidText,
                SubTitle = invalidText,
                Content = invalidText
            };

            var invalidPostViewException = 
                new InvalidPostViewException();

            invalidPostViewException.AddData(
                key: nameof(PostView.Title),
                values: "Text is required.");

            invalidPostViewException.AddData(
                key: nameof(PostView.SubTitle),
                values: "Text is required.");

            invalidPostViewException.AddData(
                key: nameof(PostView.Content), 
                values: "Text is required.");

            var expectedPostViewValidationException = 
                new PostViewValidationException(invalidPostViewException);

            // when
            ValueTask<PostView> addPostViewTask = 
                this.postViewService.AddPostViewAsync(invalidPostView);

            // then
            await Assert.ThrowsAsync<PostViewValidationException>(() => 
                addPostViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewValidationException))),
                    Times.Once());

            this.postServiceMock.Verify(broker => 
                broker.AddPostAsync(It.IsAny<Post>()), 
                Times.Never());
        } 
    }
}
