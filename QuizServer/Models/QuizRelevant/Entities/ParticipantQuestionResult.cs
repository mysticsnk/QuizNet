using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.SessionRelevant.Answers;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Services.Interfaces;

namespace QuizServer.Models.Entities.QuizRelevant;

public class ParticipantQuestionResult
{
    public Guid Id { get; set; }
    public Guid ParticipantResultId { get; set; }
    
    public Question Question { get; set; }
    public Answer Answer { get; set; }
    
    public CheckAnswerResult AnswerResult { get; set; }

    public ParticipantQuestionResult()
    {
        Id = Guid.NewGuid();
    }
    
    public ParticipantQuestionResult(Question question, Answer answer)
    {
        Id = Guid.NewGuid();
        Question = question;
        Answer = answer;
    }

    public async Task LoadAnswerResultAsync()
    {
        ICheckAnswerService answerChecker = Program.AppHost.Services.GetRequiredService<ICheckAnswerService>();
        AnswerResult = await answerChecker.CheckAnswerAsync(Question, Answer);
    }
}