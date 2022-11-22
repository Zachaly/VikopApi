using VikopApi.Application.Models.Role.Commands;
using VikopApi.Application.Models.Role.Validators;

namespace VikopApi.Tests.Unit.Validators
{
    [TestFixture]
    public class RoleValidatorTests
    {
        [Test]
        public void ValidData_PassesValidation()
        {
            var command = new AddRoleCommand
            {
                Role = "role"
            };

            var validator = new AddRoleValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void RoleEmpty_PassesValidation()
        {
            var command = new AddRoleCommand
            {
                Role = ""
            };

            var validator = new AddRoleValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void RoleExceedsMaxLength_PassesValidation()
        {
            var command = new AddRoleCommand
            {
                Role = new string('a', 11)
            };

            var validator = new AddRoleValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }
    }
}
