using System;

namespace QuizClient.Models.SessionRelevant.Answers;

public class TrueFalseAnswer : Answer
{
    public Guid SelectedOptionId { get; set; }

    public TrueFalseAnswer(Guid questionId, Guid selectedOptionId) : base(questionId)
    {
        SelectedOptionId = selectedOptionId;  
    }
}