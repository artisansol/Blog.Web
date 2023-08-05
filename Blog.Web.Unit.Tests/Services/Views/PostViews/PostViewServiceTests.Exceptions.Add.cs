using System;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;
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
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var somePostView = CreateRandomPostView();

            var expectedPostViewDependencyValidationException =
                new PostViewDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<PostView> addPostViewTask =
                this.postViewService.AddPostViewAsync(somePostView);

            // then
            await Assert.ThrowsAsync<PostViewDependencyValidationException>(() =>
                addPostViewTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.postServiceMock.Verify(service =>
                service.AddPostAsync(It.IsAny<Post>()),
                Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewDependencyValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.postServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyExceptions)
        {
            // given
            PostView somePostView = CreateRandomPostView();

            var expectedPostViewDependencyException =
                new PostViewDependencyException(dependencyExceptions);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dependencyExceptions);

            // when
            ValueTask<PostView> addPostViewTask =
                this.postViewService.AddPostViewAsync(somePostView);

            // then
            await Assert.ThrowsAsync<PostViewDependencyException>(() =>
                addPostViewTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewDependencyException))),
                    Times.Once);

            this.postServiceMock.Verify(service =>
                service.AddPostAsync(It.IsAny<Post>()),
                Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postServiceMock.VerifyNoOtherCalls();


        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var somePostView = CreateRandomPostView();
            var serviceException = new Exception();

            var failedPostViewServiceException =
                new FailedPostViewServiceException(serviceException as Xeption);

            var expectedPostViewServiceException =
                new PostViewServiceException(failedPostViewServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<PostView> addPostViewTask =
                this.postViewService.AddPostViewAsync(somePostView);

            // then
            await Assert.ThrowsAsync<PostViewServiceException>(() =>
                addPostViewTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewServiceException))),
                    Times.Once);

            this.postServiceMock.Verify(service =>
                service.AddPostAsync(It.IsAny<Post>()),
                Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postServiceMock.VerifyNoOtherCalls();
        }
    }
}
