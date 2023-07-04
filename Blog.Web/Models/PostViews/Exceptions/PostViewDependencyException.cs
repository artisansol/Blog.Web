using Xeptions;

namespace Blog.Web.Models.PostViews.Exceptions
{
    public class PostViewDependencyException : Xeption
    {
        public PostViewDependencyException(Xeption innerException)
            : base(message: "Post view dependency error occurred, contact support.", innerException)
        { }
    }
}
