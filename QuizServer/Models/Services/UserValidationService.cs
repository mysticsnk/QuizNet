using System;
using FluentValidation.Results;
using QuizServer.Models.Helpers.Password;

namespace QuizServer.Models.Services;

public class UserValidationService
{
    private PasswordValidator _passwordValidator { get; set; }

    public UserValidationService(PasswordValidator passwordValidator)
    {
        _passwordValidator = passwordValidator;
    }
    
    public bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    public bool IsValidPassword(string password)
    {
        ValidationResult result = _passwordValidator.Validate(password);
        if (result.IsValid) return true;
        
        foreach (ValidationFailure error in result.Errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }

        return false;
    }
    
    
    
    
}