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
                    .ThrowsAsync(criticalDependencyException);

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

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfBadRequestExceptionErrorOccursAndLogItAsync()
        {
            // given
            Post somePost = CreateRandomPost();
            string someMessage = GetRandomMessage();
            var someReponseMessage = new HttpResponseMessage();

            IDictionary randomDictionary = CreateRandomDictionary();
            IDictionary exceptionData = randomDictionary;
            
            var httpResponseBadRequestException = new HttpResponseBadRequestException(
                responseMessage: someReponseMessage,
                message: someMessage);

            httpResponseBadRequestException.AddData(exceptionData);

            var invalidPostException = 
                new InvalidPostException(
                    httpResponseBadRequestException, 
                    exceptionData);

            var expectedPostDependencyValidationException = 
                new PostDependencyValidationException(invalidPostException);

            this.apiBrokerMock.Setup(broker => 
                broker.PostPostAsync(somePost))
                    .ThrowsAsync(httpResponseBadRequestException);

            // when
            ValueTask<Post> addPostTask = this.postService.AddPostAsync(somePost);

            // then
            await Assert.ThrowsAsync<PostDependencyValidationException>(() => 
                addPostTask.AsTask());

            this.apiBrokerMock.Verify(broker => 
                broker.PostPostAsync(somePost), 
                    Times.Once());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyValidationException))), 
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfConflictExceptionOccursAndLogItAsync()
        {
            // given
            Post somePost = CreateRandomPost();
            IDictionary randomDictionary = CreateRandomDictionary();
            IDictionary exceptionData = randomDictionary;
            string someMessage = GetRandomMessage();
            var someRepsonseMessage = new HttpResponseMessage();

            var httpResponseConflictException =
                new HttpResponseConflictException(
                    someRepsonseMessage,
                    someMessage);

            httpResponseConflictException.AddData(exceptionData);

            var invalidPostException =
                new InvalidPostException(
                    httpResponseConflictException,
                    exceptionData);

            var expectedPostDependencyValidationException =
                new PostDependencyValidationException(invalidPostException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostPostAsync(It.IsAny<Post>()))
                    .ThrowsAsync(httpResponseConflictException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(somePost);

            // then
            await Assert.ThrowsAsync<PostDependencyValidationException>(() =>
                addPostTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowPostDependencyExceptionOnAddIfResponseExceptionOccursAndLogItAsync()
        {
            // given
            Post somePost = CreateRandomPost();
            string someMessage = GetRandomMessage();
            var httpResponseMessage = new HttpResponseMessage();

            var httpResponseException =
                new HttpResponseException(
                    httpResponseMessage,
                    someMessage);

            var failedPostDependencyException =
                new FailedPostDependencyException(httpResponseException);

            var expectedPostDependencyException =
                new PostDependencyException(failedPostDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostPostAsync(It.IsAny<Post>()))
                    .ThrowsAsync(httpResponseException);


            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(somePost);

            // then
            await Assert.ThrowsAsync<PostDependencyException>(() =>
                addPostTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Post somePost = CreateRandomPost();
            var serviceException = new Exception();

            var failedPostServiceException =
                new FailedPostServiceException(serviceException);

            var expectedPostServiceException =
                new PostServiceException(failedPostServiceException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostPostAsync(It.IsAny<Post>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(somePost);

            // then
            await Assert.ThrowsAsync<PostServiceException>(() =>
                addPostTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostServiceException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
