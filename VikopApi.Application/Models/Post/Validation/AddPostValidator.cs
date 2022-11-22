using FluentValidation;
using VikopApi.Application.Models.Post.Commands;

namespace VikopApi.Application.Models.Post.Validation
{
    public class AddPostValidator : AbstractValidator<AddPostCommand>
    {
        public AddPostValidator()
        {
            RuleFor(x => x.Content)
                .MinimumLength(10)
                .MaximumLength(700);

            RuleFor(x => x.Picture)
                .Must(x => x is null || x.ContentType == "image/jpg");
        }
    }
}
