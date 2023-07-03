using Blog.Web.Models.PostViews;
using Blog.Web.Models.PostViews.Exceptions;
using System.Threading.Tasks;
using Xeptions;

namespace Blog.Web.Services.Views.PostViews
{
    public partial class PostViewService
    {
        private delegate ValueTask<PostView> ReturningPostViewFunction();

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
        private PostViewValidationException CreateAndLogValidationException(Xeption innerException)
        {
            var postViewValidationException = new PostViewValidationException(innerException);
            this.loggingBroker.LogError(postViewValidationException);

            return postViewValidationException;
        }
    }
}
