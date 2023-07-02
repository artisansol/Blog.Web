using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Brokers.Loggings;
using Blog.Web.Services.Foundations.Posts;
using Blog.Web.Services.Views.PostViews;
using Moq;
using Tynamix.ObjectFiller;

namespace Blog.Web.Unit.Tests.Services.Views.PostViews
{
    public partial class PostViewServiceTests
    {
        private readonly Mock<IPostService> postServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IPostViewService postViewService;

        public PostViewServiceTests()
        {
            this.postServiceMock = new Mock<IPostService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.postViewService = new PostViewService(
                this.postServiceMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static dynamic CreateRandomPostViewProperties()
        {
            return new
            {
                Id = Guid.NewGuid(),
                Title = GetRandomString(),
                SubTitle = GetRandomString(),
                Content = GetRandomString(),
                Author = GetRandomString(),
                CreatedDate = GetRandomDate(),
                UpdatedDate = GetRandomDate()
            };
        }

        private static string GetRandomString() => 
            new MnemonicString().GetValue();
        private static DateTimeOffset GetRandomDate() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

    }
}
