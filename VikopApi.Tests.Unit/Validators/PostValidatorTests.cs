using Microsoft.AspNetCore.Http;
using Moq;
using VikopApi.Application.Models.Post.Commands;
using VikopApi.Application.Models.Post.Validation;

namespace VikopApi.Tests.Unit.Validators
{
    [TestFixture]
    public class PostValidatorTests
    {
        [Test]
        public void ValidData_PassesValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddPostCommand
            {
                Content = new string('a', 10),
                Picture = pictureMock.Object,
            };

            var validator = new AddPostValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void NullPicture_PassesValidation()
        {
            var command = new AddPostCommand
            {
                Content = new string('a', 10),
                Picture = null,
            };

            var validator = new AddPostValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void WrongPictureType_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/mp3");

            var command = new AddPostCommand
            {
                Content = new string('a', 10),
                Picture = pictureMock.Object,
            };

            var validator = new AddPostValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void EmptyContent_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddPostCommand
            {
                Content = "",
                Picture = pictureMock.Object,
            };

            var validator = new AddPostValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void ContentExceedsMaxLength_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddPostCommand
            {
                Content = new string('a', 701),
                Picture = pictureMock.Object,
            };

            var validator = new AddPostValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }
    }
}
