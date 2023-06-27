using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using Blog.Web.Models.Posts.Exceptions;
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
    }
}
