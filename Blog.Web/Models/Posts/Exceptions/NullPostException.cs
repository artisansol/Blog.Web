using Xeptions;

namespace Blog.Web.Models.Posts.Exceptions
{
    public class NullPostException : Xeption
    {
        public NullPostException() : base(message: "The post is null.")
        { }
    }
}
