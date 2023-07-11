using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Posts.Exceptions;
using Blog.Web.Models.PostViews;
using Blog.Web.Models.PostViews.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace Blog.Web.Unit.Tests.Services.Views.PostViews
{
    public partial class PostViewServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedPostViewDependencyException = 
                new PostViewDependencyException(dependencyException);

            this.postServiceMock.Setup(service => 
                service.RetrieveAllPostsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<PostView>> retrieveAllPostViewsTask = 
                this.postViewService.RetrieveAllPostViewsAsync();

            // then
            await Assert.ThrowsAsync<PostViewDependencyException>(() =>
                retrieveAllPostViewsTask.AsTask());

            this.postServiceMock.Verify(service => 
                service.RetrieveAllPostsAsync(), 
                Times.Once());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewDependencyException))),
                    Times.Once());

            this.postServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedPostViewServiceException = 
                new FailedPostViewServiceException(serviceException);

            var expectedPostViewServiceException = 
                new PostViewServiceException(failedPostViewServiceException);

            this.postServiceMock.Setup(service => 
                service.RetrieveAllPostsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<PostView>> retrieveAllPostViewsTask = 
                this.postViewService.RetrieveAllPostViewsAsync();

            // then
            await Assert.ThrowsAsync<PostViewServiceException>(() => 
                retrieveAllPostViewsTask.AsTask());

            this.postServiceMock.Verify(service => 
                service.RetrieveAllPostsAsync(), 
                Times.Once());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewServiceException))), 
                    Times.Once());

            this.postServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
