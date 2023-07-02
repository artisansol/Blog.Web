using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using Blog.Web.Models.PostViews;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Web.Unit.Tests.Services.Views.PostViews
{
    public partial class PostViewServiceTests
    {
        [Fact]
        public async Task ShouldRemovePostViewByIdAsync()
        {
            // given
            Guid randomPostViewId = Guid.NewGuid();
            Guid inputPostViewId = randomPostViewId;

            dynamic postViewProperties = 
                CreateRandomPostViewProperties();

            var randomPost = new Post
            {
                Id = postViewProperties.Id,
                Title = postViewProperties.Title,
                SubTitle = postViewProperties.SubTitle,
                Author = postViewProperties.Author,
                Content = postViewProperties.Content,
                CreatedDate = postViewProperties.CreatedDate,
                UpdatedDate = postViewProperties.UpdatedDate
            };

            Post removedPost = randomPost;

            var randomPostView = new PostView
            {
                Id = postViewProperties.Id,
                Title = postViewProperties.Title,
                SubTitle = postViewProperties.SubTitle,
                Author = postViewProperties.Author,
                Content = postViewProperties.Content,
                CreatedDate = postViewProperties.CreatedDate,
                UpdatedDate = postViewProperties.UpdatedDate
            };

            PostView expectedPostView = randomPostView;

            this.postServiceMock.Setup(service => 
                service.RemovePostByIdAsync(inputPostViewId))
                    .ReturnsAsync(removedPost);

            // when
            PostView actualPostView = 
                await this.postViewService.RemovePostViewByIdAsync(inputPostViewId);

            // then
            actualPostView.Should().BeEquivalentTo(expectedPostView);

            this.postServiceMock.Verify(service => 
                service.RemovePostByIdAsync(inputPostViewId),
                Times.Once());

            this.postServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
