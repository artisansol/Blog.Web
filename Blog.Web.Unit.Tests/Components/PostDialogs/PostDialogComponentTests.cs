using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Services.Views.PostViews;
using Blog.Web.Views.Components.PostDialogs;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Syncfusion.Blazor;

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
    }
}
