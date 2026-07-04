using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.ConnectionRelevant.Entities.ServerMessages;

public class AccountMessage : ServerMessage
{
    public UserAccount Account { get; set; }

    public AccountMessage(UserAccount account)
    {
        Account = account;
    }
}