using System;

namespace QuizServer.Models.Entities.QuizRelevant;

public class CheckAnswerResult
{
    public Guid Id { get; set; }
    public bool IsCorrect { get; set; }
    public int PointsGained { get; set; }

    public CheckAnswerResult()
    {
        Id = Guid.NewGuid();
    }
    
    public CheckAnswerResult(bool isCorrect, int pointsGained)
    {
        Id = Guid.NewGuid();
        IsCorrect = isCorrect;
        PointsGained = pointsGained;
    }
}