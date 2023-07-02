using System;
using System.Threading.Tasks;
using Blog.Web.Models.PostViews;

namespace Blog.Web.Services.Views.PostViews
{
    public interface IPostViewService
    {
        ValueTask<PostView> RemovePostViewByIdAsync(Guid postViewId);
    }
}
