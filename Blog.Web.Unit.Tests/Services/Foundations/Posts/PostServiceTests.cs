using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using Blog.Web.Brokers.Apis;
using Blog.Web.Brokers.Loggings;
using Blog.Web.Models.Posts;
using Blog.Web.Services.Foundations.Posts;
using Moq;
using RESTFulSense.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Blog.Web.Unit.Tests.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        private readonly Mock<IApiBroker> apiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IPostService postService;

        public PostServiceTests()
        {
            this.apiBrokerMock = new Mock<IApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.postService = new PostService(
                apiBroker: apiBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData CriticalDependencyExceptions()
        {
            string exceptionMessage = GetRandomMessage();
            var responseMessage = new HttpResponseMessage();

            var httpRequestException = 
                new HttpRequestException();

            var httpUrlNotFoundException =
                new HttpResponseUrlNotFoundException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var httpResponseUnauthorizedException = 
                new HttpResponseUnauthorizedException(
                    responseMessage: responseMessage, 
                    message: exceptionMessage);

            return new TheoryData<Exception>
            {
                httpRequestException,
                httpUrlNotFoundException,
                httpResponseUnauthorizedException
            };
        }

        private static Dictionary<string,List<string>> CreateRandomDictionary()
        {
            var filler = 
                new Filler<Dictionary<string, List<string>>>();

            return filler.Create();
        }

        private static Post CreateRandomPost() =>
            CreatePostFiller().Create();

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<Post> CreatePostFiller()
        {
            var filler = new Filler<Post>();

            filler.Setup()
                .OnType<DateTimeOffset>()
                .Use(GetRandomDateTimeOffset());

            return filler;
        }

    }
}
