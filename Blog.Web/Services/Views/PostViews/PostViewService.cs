using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Web.Brokers.DateTimes;
using Blog.Web.Brokers.Loggings;
using Blog.Web.Models.Posts;
using Blog.Web.Models.PostViews;
using Blog.Web.Services.Foundations.Posts;
using Microsoft.Extensions.Hosting;

namespace Blog.Web.Services.Views.PostViews
{
    public partial class PostViewService : IPostViewService
    {
        private readonly IPostService postService;
        private readonly ILoggingBroker loggingBroker;
        public PostViewService(IPostService postService, 
            ILoggingBroker loggingBroker)
        {
            this.postService = postService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<PostView> AddPostViewAsync(PostView postView)
        {
            Post post = MapToPost(postView);

            Post returnedPost = await this.postService.AddPostAsync(post);

            return postView;
        }

        public ValueTask<List<PostView>> RetrieveAllPostViewsAsync() =>
            TryCatch(async () =>
            {
                List<Post> retrievedPosts =
                    await this.postService.RetrieveAllPostsAsync();

                return retrievedPosts.Select(AsPostView).ToList();
            });

        public ValueTask<PostView> RemovePostViewByIdAsync(Guid postViewId) =>
            TryCatch(async () =>
            {
                ValidatePostViewId(postViewId);

                Post deletedPost = await this.postService.RemovePostByIdAsync(postViewId);

                return MapToPostView(deletedPost);
            });

        private static Func<Post, PostView> AsPostView =>
            post => MapToPostView(post);

        private static PostView MapToPostView(Post post)
        {
            return new PostView
            {
                Id = post.Id,
                Title = post.Title,
                SubTitle = post.SubTitle,
                Author = post.Author,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                UpdatedDate = post.UpdatedDate
            };
        }

        private static Post MapToPost(PostView postView)
        {
            return new Post
            {
                Id = postView.Id,
                Title = postView.Title,
                SubTitle = postView.SubTitle,
                Author = postView.Author,
                Content = postView.Content,
                CreatedDate = postView.CreatedDate,
                UpdatedDate = postView.UpdatedDate
            };
        }
    }
}
