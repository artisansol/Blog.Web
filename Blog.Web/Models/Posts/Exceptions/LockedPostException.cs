using System;
using Xeptions;

namespace Blog.Web.Models.Posts.Exceptions
{
    public class LockedPostException : Xeption
    {
        public LockedPostException(Exception innerException) :
            base(message: "Locked post error occurred, try again.",
                innerException)
        { }
    }
}
