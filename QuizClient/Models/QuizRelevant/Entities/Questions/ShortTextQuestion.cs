using QuizClient.Models.QuizRelevant.Abstracts;

namespace QuizClient.Models.QuizRelevant.Entities.Questions;

public class ShortTextQuestion : Question
{
    public string CorrectText { get; set; } = string.Empty;
    public bool CaseSensitive { get; set; }
    public string Placeholder { get; set; } = string.Empty;
    
    public ShortTextQuestion() : base() {}

    public ShortTextQuestion(string? title, byte[]? imageBytes, int pointsWeight, string? correctText,
        bool caseSensitive, string? placeholder) : base(title, imageBytes, pointsWeight)

    {
        CorrectText = correctText ?? string.Empty;
        CaseSensitive = caseSensitive;
        Placeholder = placeholder ?? string.Empty;
    }

    public bool IsCorrectAnswer(string answer)
    {
        if (CaseSensitive)
        {
            return answer == CorrectText;
        }
        else
        {
            return answer.ToLower() == CorrectText.ToLower();
        }
    }
}