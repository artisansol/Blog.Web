using Blog.Web.Models.Posts.Exceptions;
using Blog.Web.Models.PostViews;
using Blog.Web.Models.PostViews.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xeptions;

namespace Blog.Web.Services.Views.PostViews
{
    public partial class PostViewService
    {
        private delegate ValueTask<PostView> ReturningPostViewFunction();
        private delegate ValueTask<List<PostView>> ReturningPostViewsFunction();

        private async ValueTask<PostView> TryCatch(ReturningPostViewFunction returningPostViewFunction)
        {
            try
            {
                return await returningPostViewFunction();
            }
            catch (InvalidPostViewException invalidPostViewException)
            {
                throw CreateAndLogValidationException(invalidPostViewException);
            }
        }

        private async ValueTask<List<PostView>> TryCatch(ReturningPostViewsFunction returningPostViewsFunction)
        {
            try
            {
                return await returningPostViewsFunction();
            }
            catch (PostDependencyException postDependencyException)
            {
                throw CreateAndLogDependencyException(postDependencyException);
            }
            catch (PostServiceException postServiceException)
            {
                throw CreateAndLogDependencyException(postServiceException);
            }
            catch(Exception exception)
            {
                var failedPostViewServiceException = 
                    new FailedPostViewServiceException(exception);

                throw CreateAndLogServiceException(failedPostViewServiceException);
            }
        }

        private PostViewServiceException CreateAndLogServiceException(Xeption exception)
        {
            var postViewServiceException = 
                new PostViewServiceException(exception);

            this.loggingBroker.LogError(postViewServiceException);

            return postViewServiceException;
        }

        private PostViewDependencyException CreateAndLogDependencyException(Xeption innerException)
        {
            var postViewDependencyException = 
                new PostViewDependencyException(innerException);

            this.loggingBroker.LogError(postViewDependencyException);

            return postViewDependencyException;
        }
        private PostViewValidationException CreateAndLogValidationException(Xeption innerException)
        {
            var postViewValidationException = new PostViewValidationException(innerException);
            this.loggingBroker.LogError(postViewValidationException);

            return postViewValidationException;
        }
    }
}
