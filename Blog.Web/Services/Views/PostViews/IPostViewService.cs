using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Web.Models.PostViews;

namespace Blog.Web.Services.Views.PostViews
{
    public interface IPostViewService
    {
        ValueTask<List<PostView>> RetrieveAllPostViewsAsync();
        ValueTask<PostView> RemovePostViewByIdAsync(Guid postViewId);
    }
}
