using System.Text;
using Avalonia.Media.Imaging;

namespace QuizServer.Models.Entities.QuizRelevant;

public class QuestionOption
{
    public bool IsCorrect { get; init; }

    public string? TextContent { get; init; } = string.Empty;
    
    public Bitmap? ImageContent { get; init; }

    public QuestionOption(bool isCorrect, string? textContent = null, Bitmap? imageContent = null)
    {
        IsCorrect = isCorrect;
        TextContent = textContent ?? string.Empty;
        ImageContent = imageContent;
    }
    
}