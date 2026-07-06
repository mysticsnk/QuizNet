using System.Threading.Tasks;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.QuizRelevant.Abstracts;

public interface IQuizMode
{
    Task StartAsync(ServerQuizSession session);

    Task HandleAnswerAsync(Participant participant, Answer answer);

    Task NextQuestionAsync();
}