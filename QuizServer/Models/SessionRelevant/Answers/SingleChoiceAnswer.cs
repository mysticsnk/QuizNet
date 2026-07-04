using System;

namespace QuizServer.Models.SessionRelevant.Answers;

public class SingleChoiceAnswer : Answer
{
    public Guid SelectedOptionId { get; set; }
    
    public SingleChoiceAnswer(Guid participantId, Guid questionId, Guid selectedOptionId) : base(participantId,
        questionId)
    {
        SelectedOptionId = selectedOptionId;
    }
}