using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using Blog.Web.Models.Posts.Exceptions;
using Xeptions;

namespace Blog.Web.Services.Foundations.Posts
{
    public partial class PostService
    {
        private delegate ValueTask<Post> ReturningFunction();

        private async ValueTask<Post> TryCatch(ReturningFunction returningPostFunction)
        {
            try
            {
                return await returningPostFunction();
            }
            catch (NullPostException nullPostException)
            {
                throw CreateAndLogValidationException(nullPostException);
            }
        }

        private PostValidationException CreateAndLogValidationException(Xeption exception)
        {
            var postValidationException = 
                new PostValidationException(exception);

            this.loggingBroker.LogError(postValidationException);

            return postValidationException;
        }
    }
}
