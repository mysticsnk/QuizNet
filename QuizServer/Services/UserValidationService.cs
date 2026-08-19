using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using QuizServer.Services.Interfaces;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace QuizServer.Services;

public class UserValidationService : IUserValidationService
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
            throw new ValidationException(error.ErrorMessage);
        }

        return false;
    }
    
    
    
    
}