using System.Linq;
using Blog.Web.Models.PostViews.Exceptions;
using Blog.Web.Models.PostViews;
using Blog.Web.Services.Views.PostViews;
using Blog.Web.Views.Components.PostDialogs;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Syncfusion.Blazor;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using System;

namespace Blog.Web.Unit.Tests.Components.PostDialogs
{
    public partial class PostDialogComponentTests : TestContext
    {
        private readonly Mock<IPostViewService> postViewServiceMock;
        private IRenderedComponent<PostDialog> postDialogRenderedComponent;

        public PostDialogComponentTests()
        {
            this.postViewServiceMock = new Mock<IPostViewService>();
            this.Services.AddTransient(service => this.postViewServiceMock.Object);
            this.Services.AddSyncfusionBlazor();
            this.Services.AddOptions();
            this.JSInterop.Mode = JSRuntimeMode.Loose;
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string[] randomErrorMessages =
                GetRandomErrorMessages();

            var invalidPostViewException =
                new InvalidPostViewException();

            invalidPostViewException.AddData(
                key: nameof(PostView.Content),
                values: randomErrorMessages);

            return new TheoryData<Xeption> 
            {
                new PostViewValidationException(invalidPostViewException),
                new PostViewDependencyValidationException(invalidPostViewException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            var someException = new Xeption();

            return new TheoryData<Xeption>
            {
                new PostViewDependencyException(someException),
                new PostViewServiceException(someException)
            };
        }

        private static string GetRandomErrorMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string[] GetRandomErrorMessages()
        {
            int randomCount = GetRandomNumber();

            return Enumerable.Range(start: 0, count: randomCount)
                .Select(item => GetRandomErrorMessage())
                .ToArray();
        }
        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomContent() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

    }
}
