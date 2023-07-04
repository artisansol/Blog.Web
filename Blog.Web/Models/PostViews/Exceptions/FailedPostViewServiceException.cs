using System;
using Xeptions;

namespace Blog.Web.Models.PostViews.Exceptions
{
    public class FailedPostViewServiceException : Xeption
    {
        public FailedPostViewServiceException(Exception innerException)
            : base(message: "Failed post view service error occurred, contact support.", innerException)
        { }
    }
}
