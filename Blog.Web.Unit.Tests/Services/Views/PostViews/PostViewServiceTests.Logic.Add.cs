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

            dynamic randomPostViewProperty = CreateRandomPostViewProperties(
                auditDates: randomDateTime,
                auditAuthor: randomAuthor);

            var randomPostView = new PostView
            {
                Title = randomPostViewProperty.Title,
                SubTitle = randomPostViewProperty.SubTitle,
                Author = randomPostViewProperty.Author,
                Content = randomPostViewProperty.Content
            };

            PostView inputPostView = randomPostView;
            PostView expectedPostView = inputPostView;

            var randomPost = new Post
            {
                Title = randomPostViewProperty.Title,
                SubTitle = randomPostViewProperty.SubTitle,
                Author = randomPostViewProperty.Author,
                Content = randomPostViewProperty.Content
            };

            Post inputPost = randomPost;
            Post expectedPost = inputPost;

            this.datetimeBrokerMock.Setup(broker => 
                broker.GetDateTimeOffset())
                    .Returns(randomDateTime);

            this.postServiceMock.Setup(service => 
                service.AddPostAsync(inputPost))
                    .ReturnsAsync(expectedPost);

            // when
            PostView actualPostView = 
                await this.postViewService.AddPostViewAsync(inputPostView);

            // then
            actualPostView.Should().BeEquivalentTo(expectedPostView);

            this.datetimeBrokerMock.Verify(broker => 
                broker.GetDateTimeOffset(),
                    Times.Once);
            
            this.postServiceMock.Verify(service => 
                service.AddPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.datetimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postServiceMock.VerifyNoOtherCalls();
        }
    }
}
