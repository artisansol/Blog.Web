using Xeptions;

namespace Blog.Web.Models.PostViews.Exceptions
{
    public class PostViewServiceException : Xeption
    {
        public PostViewServiceException(Xeption innerException)
            : base(message: "Post view service error occurred, please try again.", innerException)
        { }
    }
}
