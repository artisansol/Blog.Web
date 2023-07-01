using Xeptions;

namespace Blog.Web.Models.Posts.Exceptions
{
    public class PostDependencyException : Xeption
    {
        public PostDependencyException(Xeption innerException) : base(
            message: "Post dependency error occurred.",
            innerException)
        { }
    }
}
