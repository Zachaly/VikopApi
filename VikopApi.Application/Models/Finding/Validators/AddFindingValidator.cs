using FluentValidation;
using VikopApi.Application.Models.Finding.Command;

namespace VikopApi.Application.Models.Finding.Validators
{
    public class AddFindingValidator : AbstractValidator<AddFindingCommand>
    {
        public AddFindingValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(200);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50);

            RuleFor(x => x.Picture)
                .Must(x => x is null || x.ContentType == "image/jpg");

            RuleFor(x => x.Link)
                .NotEmpty()
                .Matches(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
        }
    }
}
