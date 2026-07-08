using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using QuizClient.Models.QuizRelevant.Abstracts;
using QuizClient.Models.QuizRelevant.Entities.Questions;

namespace QuizClient.Models.SessionRelevant.Answers;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SingleChoiceAnswer), "single")]
[JsonDerivedType(typeof(MultiChoiceAnswer), "multi")]
[JsonDerivedType(typeof(ShortTextAnswer), "shortText")]
[JsonDerivedType(typeof(TrueFalseAnswer), "trueFalse")]
public abstract class Answer
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public TimeSpan TimeSpent { get; set; }
    private Stopwatch _stopwatch { get; set; }

    public Answer(Guid questionId)
    {
        Id = Guid.NewGuid();
        QuestionId = questionId;
    }

    public void StartTimer()
    {
        _stopwatch.Start();
    }

    public void StopTimer()
    {
        _stopwatch.Stop();
        TimeSpent = _stopwatch.Elapsed;
    }
}