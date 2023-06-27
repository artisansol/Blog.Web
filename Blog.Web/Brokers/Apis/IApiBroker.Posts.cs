using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;

namespace Blog.Web.Brokers.Apis
{
    public partial interface IApiBroker
    {
        ValueTask<Post> PostPostAsync(Post post);
        ValueTask<List<Post>> GetAllPostsAsync();
        ValueTask<Post> DeletePostByIdAsync(Guid postId);
    }
}
