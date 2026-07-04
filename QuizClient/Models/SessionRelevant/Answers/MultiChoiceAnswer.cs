using System;
using System.Collections.Generic;

namespace QuizClient.Models.SessionRelevant.Answers;

public class MultiChoiceAnswer : Answer
{
    public List<Guid> SelectedOptionIds { get; set; }

    public MultiChoiceAnswer(Guid participantId, Guid questionId, List<Guid> selectedOptionIds) : base(participantId,
        questionId)
    {
        SelectedOptionIds = selectedOptionIds;
    }
}