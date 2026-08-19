using System;
using System.Threading.Tasks;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;
using QuizServer.Services.Interfaces;

namespace QuizServer.Services;

public class DummyAnswerLogger : IAnswerLogger
{
    public Task LogAsync(Participant participant, Answer answer)
    {
        Console.WriteLine(participant.UserName);
        Console.WriteLine(answer.Id);
        Console.WriteLine("Logged answer!");

        return Task.CompletedTask;
    }
}