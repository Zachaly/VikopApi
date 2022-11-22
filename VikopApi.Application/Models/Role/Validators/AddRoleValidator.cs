using FluentValidation;
using VikopApi.Application.Models.Role.Commands;

namespace VikopApi.Application.Models.Role.Validators
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        public AddRoleValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(10);
        }
    }
}
