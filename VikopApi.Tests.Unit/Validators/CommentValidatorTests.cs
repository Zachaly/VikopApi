using Microsoft.AspNetCore.Http;
using Moq;
using VikopApi.Application.Models.Comment.Commands;
using VikopApi.Application.Models.Comment.Validators;

namespace VikopApi.Tests.Unit.Validators
{
    [TestFixture]
    public class CommentValidatorTests
    {
        [Test]
        public void AddFindingCommentCommand_ValidData_PassesValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommentCommand
            {
                Content = new string('a', 10),
                FindingId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void AddFindingCommentCommand_NullPicture_PassesValidation()
        {
            var command = new AddFindingCommentCommand
            {
                Content = new string('a', 10),
                FindingId = 1,
                Picture = null
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void AddFindingCommentCommand_ContentEmpty_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommentCommand
            {
                Content = "",
                FindingId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddFindingCommentCommand_ContentNull_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommentCommand
            {
                Content = null,
                FindingId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddFindingCommentCommand_ContentExceedsMaxLength_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddFindingCommentCommand
            {
                Content = new string('a', 501),
                FindingId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddFindingCommentCommand_WrongPictureType_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/png");

            var command = new AddFindingCommentCommand
            {
                Content = new string('a', 10),
                FindingId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddSubcommentCommand_ValidData_PassesValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddSubcommentCommand
            {
                Content = new string('a', 10),
                CommentId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void AddSubcommentCommand_NullPicture_PassesValidation()
        {
            var command = new AddSubcommentCommand
            {
                Content = new string('a', 10),
                CommentId = 1,
                Picture = null
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void AddSubcommentCommand_ContentEmpty_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddSubcommentCommand
            {
                Content = "",
                CommentId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddSubcommentCommand_ContentNull_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddSubcommentCommand
            {
                Content = null,
                CommentId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddSubcommentCommand_ContentExceedsMaxLength_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new AddSubcommentCommand
            {
                Content = new string('a', 501),
                CommentId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddSubcommentCommand_WrongPictureType_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/png");

            var command = new AddSubcommentCommand
            {
                Content = new string('a', 10),
                CommentId = 1,
                Picture = pictureMock.Object
            };
            var validator = new AddCommentValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }
    }
}
