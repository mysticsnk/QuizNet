using System;

namespace QuizServer.Models.QuizRelevant.Entities;

public class QuizListItem
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int QuestionCount { get; init; }

    public QuizListItem(Guid id, string title, int questionCount)
    {
        Id = id;
        Title = title;
        QuestionCount = questionCount;
    }
}