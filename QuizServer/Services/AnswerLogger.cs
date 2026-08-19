using System.Threading.Tasks;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;
using QuizServer.Services.Interfaces;

namespace QuizServer.Services;

public class AnswerLogger : IAnswerLogger
{
    public Task LogAsync(Participant participant, Answer answer)
    {
        throw new System.NotImplementedException();
    }
}