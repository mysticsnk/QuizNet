using System.Collections.Generic;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.UserRelevant;
using QuizServer.Models.SessionRelevant;

namespace QuizServer.Models.AppRelevant;

public class ServerState
{
    public UserAccount? Account { get; set; }
    public ServerQuizSession? CurrentSession { get; set; }
    public List<ParticipantResult> ParticipantResults { get; set; } = new();
}