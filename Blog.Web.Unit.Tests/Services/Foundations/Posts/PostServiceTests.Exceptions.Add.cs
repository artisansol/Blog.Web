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
        [Theory]
        [MemberData(nameof(CriticalDependencyExceptions))]
        public async Task ShouldThrowCriticalDependencyExeptionOnAddIfCriticalErrorOccursAndLogItAsync(Exception criticalDependencyException)
        {
            // given
            var somePost = CreateRandomPost();

            var failedPostDependencyException = 
                new FailedPostDependencyException(criticalDependencyException);

            var expectedPostDependencyException = 
                new PostDependencyException(failedPostDependencyException);

            this.apiBrokerMock.Setup(broker => 
                broker.PostPostAsync(somePost))
                    .ThrowsAsync(failedPostDependencyException);

            // when
            ValueTask<Post> addPostTask = 
                this.postService.AddPostAsync(somePost);

            // then
            await Assert.ThrowsAsync<PostDependencyException>(() => 
                addPostTask.AsTask());

            this.apiBrokerMock.Verify(broker => 
                broker.PostPostAsync(It.IsAny<Post>()), 
                    Times.Once());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostDependencyException))), 
                    Times.Once());

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
