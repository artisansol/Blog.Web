using System;
using Xeptions;

namespace Blog.Web.Models.Posts.Exceptions
{
    public class FailedPostDependencyException : Xeption
    {
        public FailedPostDependencyException(Exception innerException) : base(
            message: "Failed post dependency error occurred, contact support", 
            innerException)
        { }
    }
}
