using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizClient.Models.SessionRelevant;

namespace QuizClient.Models.Entities.QuizRelevant;

public class ParticipantResult
{
    public Guid Id { get; set; }
    public Participant Participant { get; set; }
    public List<ParticipantQuestionResult> QuestionResults { get; set; } = new();
    public int TotalScore { get; set; }
    public int? Place { get; set; }

    public ParticipantResult()
    {
        Id = Guid.NewGuid();
    }

    public ParticipantResult(Participant participant)
    {
        Id = Guid.NewGuid();
        Participant = participant;
    }

    public void AddQuestionResult(ParticipantQuestionResult questionResult)
    {
        QuestionResults.Add(questionResult);
    }

    public async Task LoadResultsAsync()
    {
        throw new NotSupportedException();
    }
    
}