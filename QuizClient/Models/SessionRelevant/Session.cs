using System.Collections.Generic;
using QuizClient.Models.Entities.QuizRelevant;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.SessionRelevant;

public class Session
{
    public List<Participant> Participants { get; set; } = new();
    public UserAccount Host { get; set; }
    public Quiz Quiz { get; set; }
}