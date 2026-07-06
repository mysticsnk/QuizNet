using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia.Media.Imaging;
using QuizClient.Models.Entities.QuizRelevant;
using QuizClient.Models.QuizRelevant.Entities.Questions;

namespace QuizClient.Models.QuizRelevant.Abstracts;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(MultiChoiceQuestion), "multi")]
[JsonDerivedType(typeof(SingleChoiceQuestion), "single")]
[JsonDerivedType(typeof(ShortTextQuestion), "shortText")]
[JsonDerivedType(typeof(TrueFalseQuestion), "trueFalse")]
public abstract class Question
{
    public Guid Id { get; set; }
    [NonSerialized] 
    public Guid QuizId;
    [NonSerialized]
    public Quiz Quiz;
    public string Title { get; set; } = string.Empty;
    
    
    public byte[]? ImageBytes { get; init; }

    public int PointsWeight { get; set; } = 100;

    public Question()
    {
        Id = Guid.NewGuid();
    }
    
    public Question(string? title, byte[]? imageBytes, int pointsWeight)
    {
        Id = Guid.NewGuid();
        Title = title ?? string.Empty;
        ImageBytes = imageBytes;
        PointsWeight = pointsWeight;
    }
}