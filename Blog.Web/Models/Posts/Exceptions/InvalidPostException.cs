using Xeptions;

namespace Blog.Web.Models.Posts.Exceptions
{
    public class InvalidPostException : Xeption
    {
        public InvalidPostException() : base(message: "Invalid post. Correct the errors and try again.")
        { }
    }
}
