using System.Collections.Generic;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;

public class RegistrationResultMessage : ServerMessage
{
    public bool IsSuccess { get; set; }
    public UserAccount? Account { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public RegistrationResultMessage() {}
    
    public RegistrationResultMessage(bool isSuccess, UserAccount? account = null, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Account = account;
        if (errors != null)
        {
            Errors = errors;
        }
    }
    
    public void AddError(string error)
    {
        Errors.Add(error);
    }
}