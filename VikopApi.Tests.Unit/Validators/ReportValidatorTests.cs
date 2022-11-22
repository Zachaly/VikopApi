using VikopApi.Application.Models.Report.Commands;
using VikopApi.Application.Models.Report.Validators;

namespace VikopApi.Tests.Unit.Validators
{
    [TestFixture]
    public class ReportValidatorTests
    {
        [Test]
        public void ValidData_PassesValidation()
        {
            var command = new AddReportCommand
            {
                Reason = new string('a', 10)
            };

            var validator = new AddReportValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void ReasionEmpty_FailsValidation()
        {
            var command = new AddReportCommand
            {
                Reason = ""
            };

            var validator = new AddReportValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void ReasionExceedsMaxLength_FailsValidation()
        {
            var command = new AddReportCommand
            {
                Reason = new string('a', 301)
            };

            var validator = new AddReportValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }
    }
}
