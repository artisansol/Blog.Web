using Blog.Web.Models.Posts;
using Blog.Web.Models.Posts.Exceptions;

namespace Blog.Web.Services.Foundations.Posts
{
    public partial class PostService
    {
        private static void ValidatePostOnAdd(Post post)
        {
            ValidatePost(post);
        }

        private static void ValidatePost(Post post)
        {
            if (post is null)
            {
                throw new NullPostException();
            }
        }
    }
}
