using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using QuizClient.Models.Entities.QuizRelevant;

namespace QuizClient.Models.QuizRelevant.Abstracts;

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