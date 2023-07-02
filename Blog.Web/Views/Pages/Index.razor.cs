using System.Threading.Tasks;
using Blog.Web.Views.Bases;
using Microsoft.AspNetCore.Components;

namespace Blog.Web.Views.Pages
{
    public partial class Index : ComponentBase
    {
        public DialogBase Dialog { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            this.Dialog.Show();
        }

        public void CloseDialog()
        {
            Dialog.Hide();
        }
    }
}
