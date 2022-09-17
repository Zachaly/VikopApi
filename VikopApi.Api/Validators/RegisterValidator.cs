﻿using FluentValidation;
using VikopApi.Api.DTO;
using VikopApi.Application.User;

namespace VikopApi.Api.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        private IsEmailOccupied _emailCheck;

        public RegisterValidator(IsEmailOccupied emailOccupied)
        {
            _emailCheck = emailOccupied;

            RuleFor(model => model.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("{PropertyValue} is not a valid e-mail address!")
                .Must(EmailCheck)
                .WithMessage("This e-mail is already occupied!");

            RuleFor(model => model.Username)
                .NotEmpty()
                .Length(10, 30)
                .WithMessage("{PropertyName} must have between {MinLength} and {MaxLength} characters!")
                .Matches("^[0-9a-zA-Z ]+$") // works for english, but does not let usage of contry specific characters like ł, cyrilic etc
                .WithMessage("{PropertyName} must consist of only alphanumeric characters!");

            RuleFor(model => model.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("{PropertyName} must have at least 6 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&].{6,}$")
                .WithMessage("{PropertyName} must have at least one lowercase and uppercase letter, number and a special character");

        }

        private bool EmailCheck(string email) => !_emailCheck.Execute(email);
    }
}