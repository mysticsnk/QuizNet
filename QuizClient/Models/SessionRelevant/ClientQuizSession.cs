using System;
using System.Collections.Generic;
using QuizClient.Models.Entities.QuizRelevant;
using QuizClient.Models.QuizRelevant.Abstracts;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.SessionRelevant;

public class ClientQuizSession
{
    public Participant Participant { get; set; }
    public Quiz Quiz { get; set; }
    public Question? CurrentQuestion { get; set; }
    public TimeSpan RemainingTime { get; set; }
    public int CurrentQuestionIndex { get; set; }
}