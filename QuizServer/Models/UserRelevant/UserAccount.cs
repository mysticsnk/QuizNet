using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using QuizServer.Models.Helpers;
using QuizServer.Models.Helpers.Password;

namespace QuizServer.Models.UserRelevant;

public class UserAccount
{
    public Guid Id { get; set; }
    
    [MaxLength(200)]
    public string UserName { get; set; }
    
    [MaxLength(20)]
    public string Password { get; set; }

    [MaxLength(100)]
    public string Email { get; set; }

    public UserAccount(string userName, string email, string password)
    {
        UserName = userName;
        Email = email;
        Password = password;
        Id = Guid.NewGuid();
    }

    public override string ToString()
    {
        return $"{UserName} : {Email}";
    }
}