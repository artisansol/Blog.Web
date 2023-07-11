using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Web.Models.PostViews;
using Blog.Web.Models.Views.Components.Timelines;
using Blog.Web.Services.Views.PostViews;
using Blog.Web.Views.Bases;
using Microsoft.AspNetCore.Components;

namespace Blog.Web.Views.Components.Timelines
{
    public partial class TimelineComponent : ComponentBase
    {
        [Inject]
        public IPostViewService PostViewService { get; set; }

        public TimelineComponentState State { get; set; }
        public List<PostView> PostViews { get; set; }
        public string ErrorMessage { get; set; }
        public LabelBase Label { get; set; }

        protected async override Task OnInitializedAsync()
        {
            this.PostViews = 
                await this.PostViewService.RetrieveAllPostViewsAsync();

            this.State = TimelineComponentState.Content;
        }

    }
}
