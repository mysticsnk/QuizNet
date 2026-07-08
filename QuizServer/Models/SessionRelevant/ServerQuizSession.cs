using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.UserRelevant;
using Question = QuizServer.Models.QuizRelevant.Abstracts.Question;

namespace QuizServer.Models.SessionRelevant;

public class ServerQuizSession
{
    public List<Participant> Participants { get; set; } = new();
    public UserAccount Host { get; set; }
    public Quiz Quiz { get; set; }
    public Question CurrentQuestion { get; set; }
    public IQuizMode Mode { get; set; }
    public TimeSpan RemainingTime { get; set; }
    public int CurrentQuestionIndex { get; set; }
    public string Pin { get; set; }

    public ServerQuizSession(Quiz quiz, string pin, IQuizMode mode)
    {
        Quiz = quiz;
        Pin = pin;
        CurrentQuestion = quiz.Questions[0];
        Mode = mode;
        CurrentQuestionIndex = 0;
    }

    public async Task StartAsync()
    {
        await Mode.StartAsync(this);
    }

    public async Task BeginQuizAsync()
    {
        
    }

    public bool IsCorrectPin(string pin)
    {
        return Pin == pin;
    }
    
    public void AddParticipant(Participant participant)
    {
        Participants.Add(participant);
    }

    public void KickParticipant(Participant participant)
    {
        Participants.Remove(participant);
    }
    
    
}