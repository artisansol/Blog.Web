using System;
using Blog.Web.Models.PostViews;
using Blog.Web.Models.PostViews.Exceptions;

namespace Blog.Web.Services.Views.PostViews
{
    public partial class PostViewService
    {
        private static void ValidatePostViewId(Guid postViewId) =>
            Validate((Rule: IsInvalid(postViewId), Parameter: nameof(PostView.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };
        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPostViewException = new InvalidPostViewException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPostViewException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPostViewException.ThrowIfContainsErrors();
        }
    }

}
