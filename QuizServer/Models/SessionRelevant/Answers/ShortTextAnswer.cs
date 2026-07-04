using System;
using System.Collections.Generic;

namespace QuizServer.Models.SessionRelevant.Answers;

public class ShortTextAnswer : Answer
{
    public string AnswerText { get; set; } = string.Empty;
    
    public ShortTextAnswer(Guid participantId, Guid questionId, string answerText) : base(participantId,
        questionId)
    {
        AnswerText = answerText;
    }
}