using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using QuizServer.Models.Entities.QuizRelevant;

namespace QuizServer.Models.QuizRelevant.Abstracts;

public abstract class Question
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public List<QuestionOption> Options { get; set; } = new ();
    
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