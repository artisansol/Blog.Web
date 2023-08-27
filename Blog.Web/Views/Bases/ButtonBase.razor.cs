using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blog.Web.Views.Bases
{
    public partial class ButtonBase : ComponentBase
    {
        [Parameter]
        public string ButtonLabel { get; set; }

        [Parameter]
        public bool IsDiabled { get; set; }

        [Parameter]
        public bool IsPrimary { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClicked { get; set; }

    }
}
