using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveIfDependencyValidationErrorOccurs(
            Xeption dependencyValidationException)
        {
            // given
            Guid somePostViewId = Guid.NewGuid();
            var expectedPostViewDependencyValidationException = 
                new PostViewDependencyValidationException(dependencyValidationException);

            this.postServiceMock.Setup(service => 
                service.RemovePostByIdAsync(somePostViewId))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<PostView> removePostViewByIdTask = 
                this.postViewService.RemovePostViewByIdAsync(somePostViewId);

            // then
            await Assert.ThrowsAsync<PostViewDependencyValidationException>(() => 
                removePostViewByIdTask.AsTask());

            this.postServiceMock.Verify(service => 
                service.RemovePostByIdAsync(It.IsAny<Guid>()),
                Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewDependencyValidationException))), 
                    Times.Once);

            this.postServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
