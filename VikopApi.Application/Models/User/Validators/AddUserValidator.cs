using FluentValidation;
using VikopApi.Application.Models.User.Requests;

namespace VikopApi.Application.Models.User.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50);
        }
    }
}
