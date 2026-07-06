using System;
using System.Collections.Generic;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.SessionRelevant;

public class ClientQuizSession
{
    public Participant Participant { get; set; }
    public Quiz Quiz { get; set; }
    public Question? CurrentQuestion { get; set; }
    public TimeSpan RemainingTime { get; set; }
    public int CurrentQuestionIndex { get; set; }
}