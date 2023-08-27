using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;

namespace Blog.Web.Views.Bases
{
    public partial class ProminentAppBarBase : ComponentBase
    {
        [Parameter]
        public AppBarColor ColorMode { get; set; }

        [Parameter]
        public AppBarPosition Position { get; set; }

        [Parameter]
        public bool IsSticky { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public RenderFragment Content { get; set; }

    }
}
