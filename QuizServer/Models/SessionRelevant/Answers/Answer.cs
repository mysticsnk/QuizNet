using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using QuizClient.Models.SessionRelevant.Answers;

namespace QuizServer.Models.SessionRelevant.Answers;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SingleChoiceAnswer), "single")]
[JsonDerivedType(typeof(MultiChoiceAnswer), "multi")]
[JsonDerivedType(typeof(ShortTextAnswer), "shortText")]
[JsonDerivedType(typeof(TrueFalseAnswer), "trueFalse")]
public abstract class Answer
{
    public Guid Id { get; set; }
    public Guid ParticipantId { get; set; }
    public Guid QuestionId { get; set; }
    public TimeSpan TimeSpent { get; set; }
    private Stopwatch _stopwatch { get; set; }

    public Answer(Guid participantId, Guid questionId)
    {
        Id = Guid.NewGuid();
        ParticipantId = participantId;
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