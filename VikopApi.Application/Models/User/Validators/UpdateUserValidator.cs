using FluentValidation;
using VikopApi.Application.Models.User.Commands;

namespace VikopApi.Application.Models.User.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50);

            RuleFor(x => x.ProfilePicture)
                .Must(x => x is null || x.ContentType == "image/jpg");
        }
    }
}
