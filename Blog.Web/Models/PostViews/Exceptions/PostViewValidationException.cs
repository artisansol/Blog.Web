using Xeptions;

namespace Blog.Web.Models.PostViews.Exceptions
{
    public class PostViewValidationException : Xeption
    {
        public PostViewValidationException(Xeption innerException)
            : base("Post view validation error occurred, try again.", innerException)
        { }
    }
}
