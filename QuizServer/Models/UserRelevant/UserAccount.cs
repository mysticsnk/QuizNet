using System;
using FluentValidation;
using QuizServer.Models.Helpers;
using QuizServer.Models.Helpers.Password;

namespace QuizServer.Models.UserRelevant;

public class UserAccount
{
    public string UserName { get; set; }
    
    public PasswordValidator PasswordValidator { get; set; }
    
    public string Password
    {
        get;
        set
        {
            PasswordValidator.ValidateAndThrow(value);
            
        }
    }

    public string Email
    {
        get;
        set
        {
            if (!EmailValidator.IsValidEmail(value))
            {
                throw new ArgumentException("Invalid email");
            }

            field = value;
        }
    }

    public UserAccount(string userName, string password)
    {
        UserName = userName;
        
        PasswordValidator = new PasswordValidator();
        try
        {
            Password = password;
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Invalid password: {ex.Message}");
        }
    }
}