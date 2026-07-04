using System.Collections.Generic;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.SessionRelevant;

public class Session
{
    public List<Participant> Participants { get; set; } = new();
    public UserAccount Host { get; set; }
    public Quiz Quiz { get; set; }
}