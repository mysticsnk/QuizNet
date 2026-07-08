using QuizServer.Models.SessionRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;

public class ClientAnswerMessage : ClientMessage
{
    public Answer Answer { get; set; }
    public Participant Participant { get; set; }
    
    public ClientAnswerMessage(Answer answer, Participant participant)
    {
        Answer = answer;
        Participant = participant;
    }
}