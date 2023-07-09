using Microsoft.AspNetCore.Components;

namespace Blog.Web.Views.Bases
{
    public partial class LabelBase : ComponentBase
    {
        [Parameter]
        public string Value { get; set; }
    }
}
