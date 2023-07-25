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
    }
}
