using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.QuizRelevant.Entities.QuizModes;

public class SynchronousQuizMode : IQuizMode
{
    
    public async Task StartAsync(ServerQuizSession session)
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        serverState.CurrentSession = session;
    }

    public async Task HandleAnswerAsync(Participant participant, Answer answer)
    {
        IAnswerLogger logger = Program.AppHost.Services.GetRequiredService<IAnswerLogger>();
        await logger.LogAsync(participant, answer);
    }

    public async Task SendCurrentQuestionAsync(Participant participant)
    {
        throw new NotSupportedException();
    }


    public async Task AdvanceQuestionAsync()
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        SocketServer server = Program.AppHost.Services.GetRequiredService<SocketServer>();
        int newIndex = serverState.CurrentSession.CurrentQuestionIndex++;
        
        if (newIndex >= serverState.CurrentSession.Quiz.Questions.Count)
        {
            await server.BroadcastQuizEndMessageAsync();
            serverState.CurrentSession = null;
            return;
        }
        
        Question newQuestion = serverState.CurrentSession.Quiz.Questions[newIndex];
        
        serverState.CurrentSession.CurrentQuestion = newQuestion;
        
        await server.BroadcastQuestionAsync(newQuestion);
    }
}