using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Application.Models.User.Commands;
using VikopApi.Application.Models.User.Requests;
using VikopApi.Application.Models.User.Validators;

namespace VikopApi.Tests.Unit.Validators
{
    [TestFixture]
    public class UserValidatorTests
    {
        [Test]
        public void AddUserRequest_ValidData_PassesValidation()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                Username = "username"
            };

            var validator = new AddUserValidator();

            var res = validator.Validate(request);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void AddUserRequest_EmailEmpty_FailsValidation()
        {
            var request = new AddUserRequest
            {
                Email = "",
                Username = "username"
            };

            var validator = new AddUserValidator();

            var res = validator.Validate(request);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddUserRequest_EmailNotValid_FailsValidation()
        {
            var request = new AddUserRequest
            {
                Email = "email",
                Username = "username"
            };

            var validator = new AddUserValidator();

            var res = validator.Validate(request);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddUserRequest_UsernameBelowMinLength_FailsValidation()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                Username = new string('a', 4)
            };

            var validator = new AddUserValidator();

            var res = validator.Validate(request);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void AddUserRequest_UsernameExceedsMaxLength_FailsValidation()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                Username = new string('a', 51)
            };

            var validator = new AddUserValidator();

            var res = validator.Validate(request);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void UpdateUserCommand_ValidData_PassesValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new UpdateUserCommand
            {
                ProfilePicture = pictureMock.Object,
                Username = "username"
            };

            var validator = new UpdateUserValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        public void UpdateUserCommand_PictureNull_PassesValidation()
        {

            var command = new UpdateUserCommand
            {
                ProfilePicture = null,
                Username = "username"
            };

            var validator = new UpdateUserValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.True);
        }

        [Test]
        [TestCase(4)]
        [TestCase(51)]
        public void UpdateUserCommand_InvalidUserNameLength_FailsValidation(int lenght)
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/jpg");

            var command = new UpdateUserCommand
            {
                ProfilePicture = pictureMock.Object,
                Username = new string('a', lenght)
            };

            var validator = new UpdateUserValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }

        [Test]
        public void UpdateUserCommand_InvalidPictureContentType_FailsValidation()
        {
            var pictureMock = new Mock<IFormFile>();
            pictureMock.Setup(x => x.ContentType).Returns("image/png");

            var command = new UpdateUserCommand
            {
                ProfilePicture = pictureMock.Object,
                Username = "username"
            };

            var validator = new UpdateUserValidator();

            var res = validator.Validate(command);

            Assert.That(res.IsValid, Is.False);
        }
    }
}
