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

        public void Show()
        {
            IsVisible = true;
            InvokeAsync(StateHasChanged);
        }

        public void Hide()
        {
            IsVisible = false;
            InvokeAsync(StateHasChanged);
        }

        public void SetValue(string value)
        {
            Label = value;
            InvokeAsync(StateHasChanged);
        }
    }
}
