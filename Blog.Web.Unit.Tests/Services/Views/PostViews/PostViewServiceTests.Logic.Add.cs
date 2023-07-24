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
            DateTimeOffset randomDateTime = GetRandomDate();
            string randomAuthor = GetRandomName();

            dynamic randomPostViewProperty = 
                CreateRandomPostViewProperties(
                    auditDates: randomDateTime,
                    auditAuthor: randomAuthor
                    );

            PostView randomPostView = new PostView
            {
                Title = randomPostViewProperty.Title,
                SubTitle = randomPostViewProperty.SubTitle,
                Author = randomPostViewProperty.Author,
                Content = randomPostViewProperty.Content,
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
                CreatedDate = randomDateTime,
                UpdatedDate = randomDateTime
            };

            Post inputPost = randomPost;
            Post expectedPost = inputPost;

            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.postServiceMock.Setup(service => 
                service.AddPostAsync(inputPost))
                    .ReturnsAsync(expectedPost);

            // when
            PostView actualPostView = 
                await this.postViewService.AddPostViewAsync(inputPostView);

            // then
            actualPostView.Should().BeEquivalentTo(expectedPostView);

            this.dateTimeBrokerMock.Verify(broker => 
                broker.GetCurrentDateTimeOffset(), 
                    Times.Once);

            this.postServiceMock.Verify(service => 
                service.AddPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.postServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
