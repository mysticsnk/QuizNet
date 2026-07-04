using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;

public class AccountMessage : ServerMessage
{
    public UserAccount Account { get; set; }

    public AccountMessage(UserAccount account)
    {
        Account = account;
    }
}