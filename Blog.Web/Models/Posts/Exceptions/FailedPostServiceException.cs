using System;
using Xeptions;

namespace Blog.Web.Models.Posts.Exceptions
{
    public class FailedPostServiceException : Xeption
    {
        public FailedPostServiceException(Exception innerException)
            : base(message: "Failed post service error occurred, contact support.",
                innerException)
        { }
    }
}
