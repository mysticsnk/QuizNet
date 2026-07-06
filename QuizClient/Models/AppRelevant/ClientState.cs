using QuizClient.Models.SessionRelevant;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.AppRelevant;

public class ClientState
{
    public UserAccount? Account { get; set; }
    public ClientQuizSession? CurrentSession { get; set; }
}