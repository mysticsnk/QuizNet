using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.QuizRelevant.Entities.QuizModes;

public class AsynchronousQuizMode : IQuizMode
{
    public async Task StartAsync(ServerQuizSession session)
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        serverState.CurrentSession = session;
    }

    public async Task HandleAnswerAsync(Participant participant, Answer answer)
    {
        throw new System.NotImplementedException();
    }

    public async Task NextQuestionAsync()
    {
        throw new System.NotImplementedException();
    }
}