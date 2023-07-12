using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldRetrieveAllPostViewsAsync()
        {
            // given
            List<dynamic> postViewsProperties =
                CreateRandomPostViewPropertiesCollection();

            List<Post> randomPosts = postViewsProperties.Select(property => new Post
            {
                Id = property.Id,
                Title = property.Title,
                SubTitle = property.SubTitle,
                Author = property.Author,
                Content = property.Content,
                CreatedDate = property.CreatedDate,
                UpdatedDate = property.UpdatedDate
            }).ToList();

            List<Post> retrievedPosts = randomPosts;

            List<PostView> randomPostViews = postViewsProperties.Select(property => new PostView
            {
                Id = property.Id,
                Title = property.Title,
                SubTitle = property.SubTitle,
                Author = property.Author,
                Content = property.Content,
                CreatedDate = property.CreatedDate,
                UpdatedDate = property.UpdatedDate
            }).ToList();

            List<PostView> expectedPostViews = randomPostViews;

            this.postServiceMock.Setup(service =>
                service.RetrieveAllPostsAsync())
                    .ReturnsAsync(retrievedPosts);

            // when
            List<PostView> retrievedPostViews =
                            await this.postViewService.RetrieveAllPostViewsAsync();

            // then
            retrievedPostViews.Should().BeEquivalentTo(expectedPostViews);

            this.postServiceMock.Verify(service =>
                service.RetrieveAllPostsAsync(),
                    Times.Once());

            this.postServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
