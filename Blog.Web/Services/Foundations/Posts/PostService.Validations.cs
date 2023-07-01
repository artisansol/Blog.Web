using System;
using System.Data;
using Blog.Web.Models.Posts;
using Blog.Web.Models.Posts.Exceptions;

namespace Blog.Web.Services.Foundations.Posts
{
    public partial class PostService
    {
        private static void ValidatePostOnAdd(Post post)
        {
            ValidatePost(post);
        }

        private static void ValidatePostId(Guid postId) =>
            Validate((Rule: IsInvalid(postId), Parameter: nameof(Post.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };

        private static void ValidatePost(Post post)
        {
            if (post is null)
            {
                throw new NullPostException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPostException = 
                new InvalidPostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPostException.UpsertDataList(
                        key: parameter, 
                        value: rule.Message);
                }
            }

            invalidPostException.ThrowIfContainsErrors();
        }

    }
}
