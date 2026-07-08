using System.Threading.Tasks;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.Services;

public class AnswerLogger : IAnswerLogger
{
    public Task LogAsync(Participant participant, Answer answer)
    {
        throw new System.NotImplementedException();
    }
}