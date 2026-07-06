using System.Collections.Generic;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.ConnectionRelevant.Entities.ServerMessages;

public class AccountResultMessage : ServerMessage
{
    public bool IsSuccess { get; set; }
    public UserAccount? Account { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public AccountResultMessage() {}
    
    public AccountResultMessage(bool isSuccess, UserAccount? account = null, List<string>? errors = null)
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