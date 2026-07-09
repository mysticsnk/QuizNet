using System.Threading.Tasks;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.Services.Interfaces;

public interface ICheckAnswerService
{
    public Task<CheckAnswerResult> CheckAnswerAsync(Question question, Answer answer);
}