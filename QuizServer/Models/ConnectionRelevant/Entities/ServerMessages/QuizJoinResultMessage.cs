using System.Collections.Generic;
using QuizServer.Models.SessionRelevant;

namespace QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;

public class QuizJoinResultMessage : ServerMessage
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new();
    public Participant? Participant { get; set; }
    public ClientQuizSession? ClientQuizSession { get; set; }

    public QuizJoinResultMessage()
    {
        
    }
    public QuizJoinResultMessage(bool isSuccess, Participant? participant = null,
        ClientQuizSession? clientQuizSession = null, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Participant = participant;
        ClientQuizSession = clientQuizSession;
        if (errors != null)
        {
            Errors = errors;
        }
    }

    public void AddError(string error)
    {
        Errors.Add(error);
    }
}