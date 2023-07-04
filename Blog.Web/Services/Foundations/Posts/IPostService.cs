using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;

namespace Blog.Web.Services.Foundations.Posts
{
    public interface IPostService
    {
        ValueTask<Post> AddPostAsync(Post post);
        ValueTask<List<Post>> RetrieveAllPostsAsync();
        ValueTask<Post> RemovePostByIdAsync(Guid postId);
    }
}
