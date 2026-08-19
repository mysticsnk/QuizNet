using System.Threading.Tasks;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Services.Interfaces;

public interface IAnswerLogger
{
    public Task LogAsync(Participant participant, Answer answer);
}