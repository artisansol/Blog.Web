using Xeptions;

namespace Blog.Web.Models.PostViews.Exceptions
{
    public class NullPostViewException : Xeption
    {
        public NullPostViewException() : base(message: "Null post error occurred.")
        { }
    }
}
