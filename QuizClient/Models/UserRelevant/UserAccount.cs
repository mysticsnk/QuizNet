using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace QuizClient.Models.UserRelevant;

public class UserAccount
{
    public Guid Id { get; set; }
    
    [MaxLength(200)]
    public string UserName { get; set; }
    
    [MaxLength(20)]
    public string PasswordHash { get; private set; }
    
    [MaxLength(100)]
    public string Email { get; set; }

    private UserAccount()
    {
        Id = Guid.NewGuid();
    }

    public UserAccount(string userName, string email, string passwordHash)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        Id = Guid.NewGuid();
    }

    public override string ToString()
    {
        return $"{UserName} : {Email}";
    }
}