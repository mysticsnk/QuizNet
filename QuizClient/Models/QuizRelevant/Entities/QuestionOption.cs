using System;
using System.Text;
using Avalonia.Media.Imaging;
using QuizClient.Models.QuizRelevant.Abstracts;

namespace QuizClient.Models.Entities.QuizRelevant;

public class QuestionOption
{
    public Guid Id { get; set; }
    
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
    public bool IsCorrect { get; init; }

    public string? TextContent { get; init; } = string.Empty;
    
    public byte[]? ImageContent { get; init; }

    public QuestionOption(bool isCorrect, string? textContent = null, byte[]? imageContent = null)
    {
        Id = Guid.NewGuid();
        IsCorrect = isCorrect;
        TextContent = textContent ?? string.Empty;
        ImageContent = imageContent;
    }
    
}