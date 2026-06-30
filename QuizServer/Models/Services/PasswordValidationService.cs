using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizServer.Models.Helpers.Password;

using FluentValidation;

public class PasswordValidationService : AbstractValidator<string>
{
    public PasswordValidationService()
    {
        // Password cannot be null or empty
        RuleFor(password => password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");

        // Password length requirements
        RuleFor(password => password)
            .Must(ValidateLength)
            .WithMessage(
                $"Password must be at least {PasswordPolicyConstants.MinimumPasswordLength} characters long and meet character type requirements");

        // At least one uppercase character
        RuleFor(password => password)
            .Must(password => password.Any(char.IsUpper))
            .WithMessage("Password must contain at least one uppercase character");
    }

    private bool ValidateLength(string password)
    {
        if (password.Length < PasswordPolicyConstants.MinimumPasswordLength)
            return false;

        return true;
    }


}