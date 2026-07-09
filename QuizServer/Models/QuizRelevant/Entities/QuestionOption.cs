using System;
using System.Text;
using System.Text.Json.Serialization;
using Avalonia.Media.Imaging;
using QuizServer.Models.QuizRelevant.Abstracts;

namespace QuizServer.Models.Entities.QuizRelevant;

public class QuestionOption
{
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid QuestionId { get; set; }
    [JsonIgnore]
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