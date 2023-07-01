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
        public async Task ShouldRemovePostById()
        {
            // given
            Guid randomPostId = Guid.NewGuid();
            Guid inputPostId = randomPostId;
            Post randomPost = CreateRandomPost();
            Post deletedPost = randomPost;
            Post expectedPost = deletedPost.DeepClone();

            this.apiBrokerMock.Setup(broker => 
                broker.DeletePostByIdAsync(inputPostId))
                    .ReturnsAsync(deletedPost);

            // when
            Post actualPost = 
                await this.postService.RemovePostByIdAsync(inputPostId);

            // then
            actualPost.Should().BeEquivalentTo(expectedPost);

            this.apiBrokerMock.Verify(broker => 
                broker.DeletePostByIdAsync(inputPostId), 
                Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
