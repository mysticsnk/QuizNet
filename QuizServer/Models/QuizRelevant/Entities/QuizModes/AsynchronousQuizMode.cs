using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.DatabaseRelevant.Entities;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;
using QuizServer.Services.Interfaces;

namespace QuizServer.Models.QuizRelevant.Entities.QuizModes;

public class AsynchronousQuizMode : IQuizMode
{
    
    public async Task StartAsync(ServerQuizSession session)
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        SocketServer server = Program.AppHost.Services.GetRequiredService<SocketServer>();
        serverState.CurrentSession = session;
    }

    public async Task HandleAnswerAsync(Participant participant, Answer answer)
    {
        IAnswerLogger logger = Program.AppHost.Services.GetRequiredService<IAnswerLogger>();
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();

        Question currentQuestion = serverState.CurrentSession.Quiz.Questions[participant.CurrentQuestionIndex];
        ParticipantQuestionResult questionResult = new ParticipantQuestionResult(currentQuestion, answer);
        serverState.ParticipantResults.FirstOrDefault(pr => pr.Participant.Id == participant.Id).AddQuestionResult(questionResult);
        
        await logger.LogAsync(participant, answer);
        await SendCurrentQuestionAsync(participant);
    }

    public async Task SendCurrentQuestionAsync(Participant participant)
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        SocketServer server = Program.AppHost.Services.GetRequiredService<SocketServer>();
        
        int newIndex = ++participant.CurrentQuestionIndex;
        participant.CurrentQuestionIndex = newIndex;

        if (newIndex >= serverState.CurrentSession.Quiz.Questions.Count)
        {
            ParticipantResult currentParticipantResult =
                serverState.ParticipantResults.FirstOrDefault(pr => pr.Participant.Id == participant.Id);
            await currentParticipantResult.LoadResultsAsync(); 
            
            await server.SendQuizEndMessageAsync(participant);
            Console.WriteLine("Quiz ended");
            return;
        }

        Question newQuestion = serverState.CurrentSession.Quiz.Questions[newIndex];
        Console.WriteLine("Sending new question...");
        await server.SendQuestionAsync(participant, newQuestion);
        Console.WriteLine("Sent new question!");
    }
}