using System.Net.Http;
using System.Threading.Tasks;
using Blog.Web.Models.Posts;
using Blog.Web.Models.Posts.Exceptions;
using RESTFulSense.Exceptions;
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
            catch (InvalidPostException invalidPostException)
            {
                throw CreateAndLogValidationException(invalidPostException);
            }
            catch (HttpRequestException httpRequestException)
            {
                var failedPostDependencyException = 
                    new FailedPostDependencyException(httpRequestException);

                throw CreateAndLogCriticalDependencyException(failedPostDependencyException);
            }
            catch(HttpResponseUrlNotFoundException httpUrlNotFoundException)
            {
                var failedPostDependencyException = 
                    new FailedPostDependencyException(httpUrlNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedPostDependencyException);
            }
            catch(HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var failedPostDependencyException = 
                    new FailedPostDependencyException(httpResponseUnauthorizedException);

                throw CreateAndLogCriticalDependencyException(failedPostDependencyException);
            }
            catch(HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var notFoundException = 
                    new NotFoundException(httpResponseNotFoundException);

                throw CreateAndLogDependencyValidationException(notFoundException);
            }
            catch(HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidPostException = 
                    new InvalidPostException(
                        httpResponseBadRequestException,
                        httpResponseBadRequestException.Data);

                throw CreateAndLogDependencyValidationException(invalidPostException);
            }
        }

        private PostDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var postDependencyValidationException = 
                new PostDependencyValidationException(exception);

            this.loggingBroker.LogError(postDependencyValidationException);

            return postDependencyValidationException;
        }

        private PostDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var postDependencyException = 
                new PostDependencyException(exception);

            this.loggingBroker.LogCritical(postDependencyException);

            return postDependencyException;
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
