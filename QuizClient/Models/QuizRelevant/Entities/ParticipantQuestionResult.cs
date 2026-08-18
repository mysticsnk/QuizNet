using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizClient.Models.SessionRelevant.Answers;
using QuizClient.Models.QuizRelevant.Abstracts;
using QuizClient.Models.Services.Interfaces;

namespace QuizClient.Models.Entities.QuizRelevant;

public class ParticipantQuestionResult
{
    public Guid Id { get; set; }
    public Guid ParticipantResultId { get; set; }
    
    public Question Question { get; set; }
    public Answer Answer { get; set; }
    
    public CheckAnswerResult AnswerResult { get; set; }

    public ParticipantQuestionResult()
    {
        
    }
    
    public ParticipantQuestionResult(Question question, Answer answer)
    {
        Question = question;
        Answer = answer;
    }

    public async Task LoadAnswerResultAsync()
    {
        throw new NotSupportedException();
    }
}