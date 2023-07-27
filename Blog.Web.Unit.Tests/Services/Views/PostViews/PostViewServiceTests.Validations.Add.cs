using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using Blog.Web.Models.PostViews;
using Blog.Web.Models.PostViews.Exceptions;
using Moq;
using Xunit;

namespace Blog.Web.Unit.Tests.Services.Views.PostViews
{
    public partial class PostViewServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPostViewIdIsNullAndLogItAsync()
        {
            // given
            PostView nullPostView = null;
            var nullPostViewException = new NullPostViewException();

            var expectedPostViewValidationException = 
                new PostViewValidationException(nullPostViewException);

            // when
            ValueTask<PostView> addPostViewTask = 
                this.postViewService.AddPostViewAsync(nullPostView);

            // then
            await Assert.ThrowsAsync<PostViewValidationException>(() => 
                addPostViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostViewValidationException))),
                    Times.Once);

            this.postServiceMock.Verify(service => 
                service.AddPostAsync(It.IsAny<Post>()), 
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
