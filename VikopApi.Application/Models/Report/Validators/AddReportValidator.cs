using FluentValidation;
using VikopApi.Application.Models.Report.Commands;

namespace VikopApi.Application.Models.Report.Validators
{
    public class AddReportValidator : AbstractValidator<AddReportCommand>
    {
        public AddReportValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(300);
        }
    }
}
