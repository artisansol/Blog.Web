using System;
using System.Threading.Tasks;
using Blog.Web.Brokers.Loggings;
using Blog.Web.Models.PostViews;
using Blog.Web.Services.Foundations.Posts;

namespace Blog.Web.Services.Views.PostViews
{
    public class PostViewService : IPostViewService
    {
        private readonly IPostService postService;
        private readonly ILoggingBroker loggingBroker;

        public PostViewService(IPostService postService, ILoggingBroker loggingBroker)
        {
            this.postService = postService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<PostView> RemovePostViewByIdAsync(Guid postViewId)
        {
            throw new NotImplementedException();
        }
    }
}
