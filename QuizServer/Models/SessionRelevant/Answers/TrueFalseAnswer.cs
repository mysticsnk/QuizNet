using System;

namespace QuizServer.Models.SessionRelevant.Answers;

public class TrueFalseAnswer : Answer
{
    public Guid SelectedOptionId { get; set; }

    public TrueFalseAnswer(Guid participantId, Guid questionId, Guid selectedOptionId) : base(participantId,
        questionId)
    {
        SelectedOptionId = selectedOptionId;  
    }
}