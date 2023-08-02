using System.Linq;
using Blog.Web.Services.Views.PostViews;
using Blog.Web.Views.Components.PostDialogs;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Syncfusion.Blazor;
using Tynamix.ObjectFiller;

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
