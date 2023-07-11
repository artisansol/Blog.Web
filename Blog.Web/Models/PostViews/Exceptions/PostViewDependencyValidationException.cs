using Xeptions;

namespace Blog.Web.Models.PostViews.Exceptions
{
    public class PostViewDependencyValidationException : Xeption
    {
        public PostViewDependencyValidationException(Xeption innerException)
            : base(message: "Post view dependency validation error occured, please try again.", innerException)
        { }
    }
}
