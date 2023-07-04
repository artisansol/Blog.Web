using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace Blog.Web.Unit.Tests.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllPostsAsync()
        {
            // given
            List<Post> randomPosts = CreateRandomPosts();
            List<Post> storagePosts = randomPosts;
            List<Post> expectedPosts = storagePosts.DeepClone();

            this.apiBrokerMock.Setup(broker => 
                broker.GetAllPostsAsync())
                    .ReturnsAsync(expectedPosts);

            // when
            List<Post> retrievedPosts = 
                await this.postService.RetrieveAllPostsAsync();

            // then
            retrievedPosts.Should().BeEquivalentTo(expectedPosts);

            this.apiBrokerMock.Verify(broker => 
                broker.GetAllPostsAsync(),
                Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
