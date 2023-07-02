using System;
using System.Collections;
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

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveIfValidationErrorOccursAndLogItAsync()
        {
            // given
            Guid somePostId = Guid.NewGuid();
            IDictionary randomDictionary = CreateRandomDictionary();
            IDictionary validationErrorsDictionary = randomDictionary;

            string responseMessage = 
                GetRandomMessage();

            var httpResponseMessage = 
                new HttpResponseMessage();

            var httpResponseBadRequestException = 
                new HttpResponseBadRequestException(
                    responseMessage: httpResponseMessage,
                    message: responseMessage);

            httpResponseBadRequestException.AddData(validationErrorsDictionary);

            var invalidPostException = 
                new InvalidPostException(
                    httpResponseBadRequestException,
                    data: validationErrorsDictionary);

            var expectedPostDependencyValidationException = 
                new PostDependencyValidationException(invalidPostException);

            this.apiBrokerMock.Setup(broker => 
                broker.DeletePostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(httpResponseBadRequestException);

            // when
            ValueTask<Post> removePostByIdTask = 
                this.postService.RemovePostByIdAsync(somePostId);

            // then
            await Assert.ThrowsAsync<PostDependencyValidationException>(() => 
                removePostByIdTask.AsTask());

            this.apiBrokerMock.Verify(broker => 
                broker.DeletePostByIdAsync(It.IsAny<Guid>()),
                Times.Once());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyValidationException))),
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveIfPostIsLockedAndLogItAsync()
        {
            // given
            Guid somePostId = Guid.NewGuid();
            string responseMessage = GetRandomMessage();
            var httpResponseMessage = new HttpResponseMessage();

            var httpResponseLockedException = 
                new HttpResponseLockedException(
                    httpResponseMessage, 
                    message: responseMessage);

            var lockedPostException = 
                new LockedPostException(httpResponseLockedException);

            var expectedPostDependencyValidationException = 
                new PostDependencyValidationException(lockedPostException);

            this.apiBrokerMock.Setup(broker => 
                broker.DeletePostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(httpResponseLockedException);

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

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveIfDependencyErrorOccursAndLogItAsync()
        {
            // given
            Guid somePostId = Guid.NewGuid();
            string someMessage = GetRandomMessage();

            var httpResponseMessage = 
                new HttpResponseMessage();

            var httpResponseException = 
                new HttpResponseException(
                    httpResponseMessage: httpResponseMessage, 
                    message: someMessage);

            var failedPostDependencyException = 
                new FailedPostDependencyException(httpResponseException);

            var expectedPostDependencyException = 
                new PostDependencyException(failedPostDependencyException);

            this.apiBrokerMock.Setup(broker => 
                broker.DeletePostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(httpResponseException);

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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
