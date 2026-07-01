using System;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.SessionRelevant;

public class Participant
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public int Points { get; set; }
    public UserAccount? Account { get; set; }

    public Participant(string userName, UserAccount? account)
    {
        UserName = userName;
        if (account != null)
        {
            Account = account;
            Id = Account.Id;
        }
        else
        {
            Id = Guid.NewGuid();
        }
    }
}