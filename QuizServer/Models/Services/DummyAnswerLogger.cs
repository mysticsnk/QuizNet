using System;
using System.Threading.Tasks;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.Services;

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