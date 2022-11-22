using Microsoft.AspNetCore.Http;
using Moq;
using VikopApi.Application.Models.Finding.Command;
using VikopApi.Application.Models.Finding.Validators;

namespace VikopApi.Tests.Unit.Validators
{
    [TestFixture]
    public class FindingValidatorTests
    {
        [Test]
        public void ValidData_PassesValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommand
            {
                Description = new string('a', 10),
                Link = "https://link.com",
                Picture = pictureMock.Object,
                Title = new string('a', 5)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void NullPicture_PassesValidation()
        {
            var command = new AddFindingCommand
            {
                Description = new string('a', 10),
                Link = "https://link.com",
                Picture = null,
                Title = new string('a', 5)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void HttpLink_PassesValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommand
            {
                Description = new string('a', 10),
                Link = "http://link.com",
                Picture = pictureMock.Object,
                Title = new string('a', 5)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void EmptyDescription_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommand
            {
                Description = "",
                Link = "https://link.com",
                Picture = pictureMock.Object,
                Title = new string('a', 5)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void DescriptionExceedsMaxLength_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommand
            {
                Description = new string('a', 201),
                Link = "https://link.com",
                Picture = pictureMock.Object,
                Title = new string('a', 5)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void EmptyTitle_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommand
            {
                Description = new string('a', 10),
                Link = "https://link.com",
                Picture = pictureMock.Object,
                Title = ""
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void TitleExceedsMaxLength_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommand
            {
                Description = new string('a', 10),
                Link = "https://link.com",
                Picture = pictureMock.Object,
                Title = new string('a', 51)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void LinkEmpty_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommand
            {
                Description = new string('a', 10),
                Link = "",
                Picture = pictureMock.Object,
                Title = new string('a', 5)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void LinkNotValid_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommand
            {
                Description = new string('a', 10),
                Link = "abc",
                Picture = pictureMock.Object,
                Title = new string('a', 5)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void WrongPictureType_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/mp3");

            var command = new AddFindingCommand
            {
                Description = new string('a', 10),
                Link = "https://link.com",
                Picture = pictureMock.Object,
                Title = new string('a', 5)
            };
            var validator = new AddFindingValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }
    }
}
