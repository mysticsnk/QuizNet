using System;

namespace QuizClient.Models.SessionRelevant.Answers;

public class SingleChoiceAnswer : Answer
{
    public Guid SelectedOptionId { get; set; }
    
    public SingleChoiceAnswer(Guid questionId, Guid selectedOptionId) : base(questionId)
    {
        SelectedOptionId = selectedOptionId;
    }
}