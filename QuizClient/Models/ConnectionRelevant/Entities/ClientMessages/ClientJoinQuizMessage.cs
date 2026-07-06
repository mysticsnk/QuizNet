using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.ConnectionRelevant.Entities.ClientMessages;

public class ClientJoinQuizMessage : ClientMessage
{
    public UserAccount? Account { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;

    public ClientJoinQuizMessage(string userName, string pin, UserAccount? account = null)
    {
        UserName = userName;
        Pin = pin;
        Account = account;
    }
}