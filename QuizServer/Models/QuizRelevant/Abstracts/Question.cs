using Avalonia.Media.Imaging;
using QuizServer.Models.Entities.QuizRelevant;

namespace QuizServer.Models.QuizRelevant.Abstracts;

public abstract class Question
{
    public string Title { get; set; } = string.Empty;
    
    public Bitmap? Image { get; init; }

    public int PointsWeight { get; set; } = 100;

    public Question(string? title, Bitmap? image, int pointsWeight)
    {
        Title = title ?? string.Empty;
        Image = image;
        PointsWeight = pointsWeight;
    }
}