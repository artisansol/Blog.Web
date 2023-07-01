using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using Blog.Web.Models.Posts.Exceptions;
using Moq;
using RESTFulSense.Exceptions;
using Xunit;

namespace Blog.Web.Unit.Tests.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyExceptions))]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfCriticalErrorOccursAndLogItAsync(
            Exception criticalDependencyException)
        {
            // given
            Guid somePostId = Guid.NewGuid();

            var failedPostDependencyException = 
                new FailedPostDependencyException(criticalDependencyException);

            var expectedPostDependencyException = 
                new PostDependencyException(failedPostDependencyException);

            this.apiBrokerMock.Setup(broker => 
                broker.DeletePostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<Post> removePostByIdTask = 
                this.postService.RemovePostByIdAsync(somePostId);

            // then
            await Assert.ThrowsAsync<PostDependencyException>(() => 
                removePostByIdTask.AsTask());

            this.apiBrokerMock.Verify(broker => 
                broker.DeletePostByIdAsync(It.IsAny<Guid>()), 
                Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostDependencyException))), 
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveIfPostIsNotFoundAndLogItAsync()
        {
            // given
            Guid somePostId = Guid.NewGuid();
            string responseMessage = GetRandomMessage();
            var httpResponseMessage = new HttpResponseMessage();

            var httpResponseNotFoundException = 
                new HttpResponseNotFoundException(
                    responseMessage: httpResponseMessage, 
                    message: responseMessage);

            var notFoundException = 
                new NotFoundException(httpResponseNotFoundException); 

            var expectedPostDependencyValidationException = 
                new PostDependencyValidationException(notFoundException);

            this.apiBrokerMock.Setup(broker => 
                broker.DeletePostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(httpResponseNotFoundException);

            // when
            ValueTask<Post> removePostByIdTask = 
                this.postService.RemovePostByIdAsync(somePostId);

            // then
            await Assert.ThrowsAsync<PostDependencyValidationException>(() => 
                removePostByIdTask.AsTask());

            this.apiBrokerMock.Verify(broker => 
                broker.DeletePostByIdAsync(It.IsAny<Guid>()), 
                Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyValidationException))), 
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
