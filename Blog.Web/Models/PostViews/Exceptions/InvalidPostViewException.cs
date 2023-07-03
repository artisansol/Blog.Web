using Xeptions;

namespace Blog.Web.Models.PostViews.Exceptions
{
    public class InvalidPostViewException : Xeption
    {
        public InvalidPostViewException()
            : base(message: "Invalid post view, correct the error and try again.")
        { }
    }
}
