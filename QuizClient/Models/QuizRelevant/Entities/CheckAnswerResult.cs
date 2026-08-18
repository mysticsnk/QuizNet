namespace QuizClient.Models.Entities.QuizRelevant;

public class CheckAnswerResult
{
    public bool IsCorrect { get; set; }
    public int PointsGained { get; set; }

    public CheckAnswerResult()
    {
        
    }
    
    public CheckAnswerResult(bool isCorrect, int pointsGained)
    {
        IsCorrect = isCorrect;
        PointsGained = pointsGained;
    }
}