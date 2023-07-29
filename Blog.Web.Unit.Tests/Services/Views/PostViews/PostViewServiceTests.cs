using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Blog.Web.Brokers.DateTimes;
using Blog.Web.Brokers.Loggings;
using Blog.Web.Models.Posts;
using Blog.Web.Models.Posts.Exceptions;
using Blog.Web.Models.PostViews;
using Blog.Web.Services.Foundations.Posts;
using Blog.Web.Services.Views.PostViews;
using KellermanSoftware.CompareNetObjects;
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
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IPostViewService postViewService;
        
        public PostViewServiceTests()
        {
            this.postServiceMock = new Mock<IPostService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            var compareConfig = new ComparisonConfig();
            compareConfig.IgnoreProperty<Post>(post => post.Id);
            this.compareLogic = new CompareLogic(compareConfig);

            this.postViewService = new PostViewService(
                postService: this.postServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        public static TheoryData DependencyExceptions()
        {
            var innerException = new Xeption();

            var postDependencyException =
                new PostDependencyException(innerException);

            var postServiceException =
                new PostServiceException(innerException);

            return new TheoryData<Xeption>
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

        public static TheoryData DependencyValidationExceptions()
        {
            var innerException = new Xeption();

            var postServiceValidationException = 
                new PostValidationException(innerException);

            var postDependencyValidation = 
                new PostDependencyValidationException(innerException);

            return new TheoryData<Xeption>
            {
                postServiceValidationException,
                postDependencyValidation
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

        private static dynamic CreateRandomPostViewProperties(
            DateTimeOffset auditDates,
            string auditAuthor)
        {
            return new
            {
                Id = Guid.NewGuid(),
                Title = GetRandomString(),
                SubTitle = GetRandomString(),
                Content = GetRandomString(),
                Author = auditAuthor,
                CreatedDate = auditDates,
                UpdatedDate = auditDates
            };
        }

        private static PostView CreateRandomPostView() =>
            CreateRandomPostViewFiller().Create();

        private static Filler<PostView> CreateRandomPostViewFiller()
        {
            var filler = new Filler<PostView>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
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
        private Expression<Func<Post, bool>> SamePostAs(Post expectedPost) =>
            actualPost => this.compareLogic.Compare(actualPost, expectedPost).AreEqual;
    }
}
