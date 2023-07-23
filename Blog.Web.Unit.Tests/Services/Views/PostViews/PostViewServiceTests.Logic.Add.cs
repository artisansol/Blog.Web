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
        public async Task ShouldAddPostViewAsync()
        {
            // given
            dynamic randomPostViewProperty = 
                CreateRandomPostViewProperties();

            PostView randomPostView = new PostView
            {
                Id = randomPostViewProperty.Id,
                Title = randomPostViewProperty.Title,
                SubTitle = randomPostViewProperty.SubTitle,
                Author = randomPostViewProperty.Author,
                Content = randomPostViewProperty.Content,
                CreatedDate = randomPostViewProperty.CreatedDate,
                UpdatedDate = randomPostViewProperty.UpdatedDate
            };

            PostView inputPostView = randomPostView;
            PostView expectedPostView = inputPostView;

            var randomPost = new Post
            {
                Id = randomPostViewProperty.Id,
                Title = randomPostViewProperty.Title,
                SubTitle = randomPostViewProperty.SubTitle,
                Author = randomPostViewProperty.Author,
                Content = randomPostViewProperty.Content,
                CreatedDate = randomPostViewProperty.CreatedDate,
                UpdatedDate = randomPostViewProperty.UpdatedDate
            };

            Post inputPost = randomPost;
            Post expectedPost = inputPost;

            this.postServiceMock.Setup(service => 
                service.AddPostAsync(inputPost))
                    .ReturnsAsync(expectedPost);

            // when
            PostView actualPostView = 
                await this.postViewService.AddPostViewAsync(inputPostView);

            // then
            actualPostView.Should().BeEquivalentTo(expectedPostView);

            this.postServiceMock.Verify(service => 
                service.AddPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.postServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
