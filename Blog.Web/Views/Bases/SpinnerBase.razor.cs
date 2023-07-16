using System;
using Microsoft.AspNetCore.Components;

namespace Blog.Web.Views.Bases
{
    public partial class SpinnerBase : ComponentBase
    {
        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string Size { get; set; }

    }
}
