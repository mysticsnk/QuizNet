using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizServer.Models.SessionRelevant;

namespace QuizServer.Models.Entities.QuizRelevant;

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
        List<Task> tasks = new();
        foreach (var result in QuestionResults)
        {
            tasks.Add(result.LoadAnswerResultAsync());
        }

        await Task.WhenAll(tasks);

        TotalScore = 0;

        foreach (var result in QuestionResults)
        {
            TotalScore += result.AnswerResult.PointsGained;
        }
    }
    
}