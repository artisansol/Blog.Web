using Blog.Web.Models.Posts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Blog.Web.Brokers.Apis
{
    public partial class ApiBroker
    {
        private const string PostsRelativeUrl = "api/posts";
        public async ValueTask<Post> PostPostAsync(Post post) =>
            await this.PostAsync(PostsRelativeUrl, post);

        public async ValueTask<List<Post>> GetAllPostsAsync() =>
            await this.GetAsync<List<Post>>(PostsRelativeUrl);

        public async ValueTask<Post> DeletePostByIdAsync(Guid postId) =>
            await this.DeleteAsync<Post>($"{PostsRelativeUrl}/{postId}");
    }
}
