using Xeptions;

namespace Blog.Web.Models.Posts.Exceptions
{
    public class PostDependencyValidationException : Xeption
    {
        public PostDependencyValidationException(Xeption innerException) : base(
            message: "Post dependency validation error occurred, please try again.",
            innerException)
        { }
    }
}
