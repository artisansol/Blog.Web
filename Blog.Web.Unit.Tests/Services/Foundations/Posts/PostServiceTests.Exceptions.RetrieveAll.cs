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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfCriticalErrorOccursAndLogItAsync(
            Exception criticalDependencyException)
        {
            // given
            var failedPostDependencyException = 
                new FailedPostDependencyException(criticalDependencyException);

            var expectedPostDependencyException = 
                new PostDependencyException(failedPostDependencyException);

            this.apiBrokerMock.Setup(broker => 
                broker.GetAllPostsAsync())
                    .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<List<Post>> retrieveAllPostsTask = 
                this.postService.RetrieveAllPostsAsync();

            // then
            await Assert.ThrowsAsync<PostDependencyException>(() => 
                retrieveAllPostsTask.AsTask());

            this.apiBrokerMock.Verify(broker => 
                broker.GetAllPostsAsync(), 
                Times.Once());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostDependencyException))), 
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
