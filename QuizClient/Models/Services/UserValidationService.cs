using System;
using FluentValidation.Results;
using QuizClient.Models.Helpers.Password;

namespace QuizClient.Models.Services;

public class UserValidationService
{
    private PasswordValidationService PasswordValidationService { get; set; }

    public UserValidationService(PasswordValidationService passwordValidationService)
    {
        PasswordValidationService = passwordValidationService;
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
        ValidationResult result = PasswordValidationService.Validate(password);
        if (result.IsValid) return true;
        
        foreach (ValidationFailure error in result.Errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }

        return false;
    }
    
    
    
    
}