using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Brokers.Loggings;
using Blog.Web.Models.Posts.Exceptions;
using Blog.Web.Models.PostViews.Exceptions;
using Blog.Web.Services.Foundations.Posts;
using Blog.Web.Services.Views.PostViews;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

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

        public static TheoryData DependencyExceptions()
        {
            var innerException = new Xeption();

            var postDependencyException = 
                new PostDependencyException(innerException);

            var postServiceException = 
                new PostServiceException(innerException);

            return new TheoryData<Exception>
            {
                postDependencyException, 
                postServiceException
            };
        }

        public static TheoryData ValidationExceptions()
        {
            var innerException = new Xeption();

            var postValidationException =
                new PostValidationException(innerException);

            var postDependencyValidationException =
                new PostDependencyValidationException(innerException);

            return new TheoryData<Exception>
            {
                postValidationException,
                postDependencyValidationException
            };
        }

        private static List<dynamic> CreateRandomPostViewPropertiesCollection() 
        {
            int randomCount = GetRandomNumber();

            return Enumerable.Range(0, randomCount).Select(item =>
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
            }).ToList<dynamic>();
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

        private static string GetRandomName() => 
            new RealNames().GetValue();

        private static int GetRandomNumber() => 
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() => 
            new MnemonicString().GetValue();
        private static DateTimeOffset GetRandomDate() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
