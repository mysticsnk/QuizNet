using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.DatabaseRelevant.Entities;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;
using QuizServer.Services.Interfaces;

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
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();

        Question currentQuestion = serverState.CurrentSession.CurrentQuestion;
        ParticipantQuestionResult questionResult = new ParticipantQuestionResult(currentQuestion, answer);
        
        serverState.ParticipantResults.FirstOrDefault(pr => pr.Participant.Id == participant.Id).AddQuestionResult(questionResult);
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
            List<Task> tasks = new();
            foreach (ParticipantResult result in serverState.ParticipantResults)
            {
                tasks.Add(result.LoadResultsAsync());
            }
            await Task.WhenAll(tasks);
            
            serverState.ParticipantResults =
                serverState.ParticipantResults.OrderByDescending(pr => pr.TotalScore).ToList();

            for (int i = 1; i <= serverState.ParticipantResults.Count; i++)
            {
                serverState.ParticipantResults[i - 1].Place = i;
            }
            
            await server.BroadcastQuizEndMessageAsync();
            serverState.CurrentSession = null;
            Console.WriteLine("Quiz ended");
            return;
        }
        
        Question newQuestion = serverState.CurrentSession.Quiz.Questions[newIndex];
        
        serverState.CurrentSession.CurrentQuestion = newQuestion;
        
        await server.BroadcastQuestionAsync(newQuestion);
    }
}