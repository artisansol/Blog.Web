using System;
using Xeptions;

namespace Blog.Web.Models.Posts.Exceptions
{
    public class NotFoundException : Xeption
    {
        public NotFoundException(Exception innerException) : base(
            message: "Not found post error occurred, please try again.",
            innerException)
        { }
    }
}
