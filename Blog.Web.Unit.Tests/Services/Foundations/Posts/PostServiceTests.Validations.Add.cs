using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using Blog.Web.Models.Posts.Exceptions;
using Blog.Web.Models.PostViews;
using Moq;
using Xunit;

namespace Blog.Web.Unit.Tests.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPostIsNullAndLogItAsync()
        {
            // given
            Post nullPost = null;

            var nullPostException =
                new NullPostException();

            var expectedPostValidationException =
                new PostValidationException(nullPostException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(nullPost);

            // then
            await Assert.ThrowsAsync<PostValidationException>(() =>
                addPostTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                    Times.Once);

            this.apiBrokerMock.Verify(broker =>
                broker.PostPostAsync(It.IsAny<Post>()),
                Times.Never);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddIfPostIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            Post invalidPost = new Post
            {
                Author = invalidText,
                Title = invalidText,
                SubTitle = invalidText,
                Content = invalidText
            };

            var invalidPostException = new InvalidPostException();

            invalidPostException.AddData(key: nameof(Post.Content),
                values: "Text is required.");

            invalidPostException.AddData(key: nameof(Post.Title),
                values: "Text is required.");

            invalidPostException.AddData(key: nameof(Post.SubTitle),
                values: "Text is required.");

            invalidPostException.AddData(key: nameof(Post.Author),
                values: "Text is required.");

            var expectedPostValidationException = 
                new PostValidationException(invalidPostException);

            // when
            ValueTask<Post> addPostTask = 
                this.postService.AddPostAsync(invalidPost);

            // then
            await Assert.ThrowsAsync<PostValidationException>(() => 
                addPostTask.AsTask());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))), 
                    Times.Once);

            this.apiBrokerMock.Verify(broker => 
                broker.PostPostAsync(It.IsAny<Post>()), 
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }
}
