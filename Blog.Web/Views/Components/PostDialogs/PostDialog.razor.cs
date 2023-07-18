using System;
using System.Threading.Tasks;
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
        public PostDialogComponentState State { get; set; }
        public DialogBase Dialog { get; set; }
        public bool IsVisible { get; set; }

        protected override void OnInitialized() =>
            this.State = PostDialogComponentState.Content;

        public void OpenDialog() 
        {
            this.Dialog.Show();
            this.IsVisible = true;
        }
            

    }
}
