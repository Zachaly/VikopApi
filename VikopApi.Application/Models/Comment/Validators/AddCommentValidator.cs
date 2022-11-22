using FluentValidation;
using VikopApi.Application.Models.Comment.Commands;

namespace VikopApi.Application.Models.Comment.Validators
{
    public class AddCommentValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(500);

            RuleFor(x => x.Picture)
                .Must(x => x is null || x.ContentType == "image/jpg");
        }
    }
}
