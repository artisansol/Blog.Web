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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidPostViewId = Guid.Empty;

            var invalidPostViewException = 
                new InvalidPostViewException();

            invalidPostViewException.AddData(
                key: nameof(PostView.Id), 
                values: "Id is required.");

            var expectedPostViewValidationException = 
                new PostViewValidationException(invalidPostViewException);

            // when
            ValueTask<PostView> removePostViewByIdTask = 
                this.postViewService.RemovePostViewByIdAsync(invalidPostViewId);

            // then
            await Assert.ThrowsAsync<PostViewValidationException>(() => 
                removePostViewByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewValidationException))), 
                    Times.Once);

            this.postServiceMock.Verify(service => 
                service.RemovePostByIdAsync(It.IsAny<Guid>()), 
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postServiceMock.VerifyNoOtherCalls();
        }

    }
}
