using Microsoft.AspNetCore.Components;

namespace Blog.Web.Views.Bases
{
    public partial class CardBase : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string SubTitle { get; set; }

        [Parameter]
        public RenderFragment Content { get; set; }

        [Parameter]
        public RenderFragment Footer { get; set; }
    }
}
