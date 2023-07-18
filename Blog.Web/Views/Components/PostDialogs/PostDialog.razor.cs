using Blog.Web.Models.Views.Components.PostDialogs;
using Blog.Web.Services.Views.PostViews;
using Blog.Web.Views.Bases;
using Microsoft.AspNetCore.Components;

namespace Blog.Web.Views.Components.PostDialogs
{
    public partial class PostDialog : ComponentBase
    {
        [Inject]
        public IPostViewService PostViewService { get; set; }
        public PostDialogComponentState ComponentState { get; set; }
        public DialogBase Dialog { get; set; }
        public bool IsVisible { get; set; }
    }
}
