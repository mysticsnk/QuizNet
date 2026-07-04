using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;

public class ClientAnswerMessage : ClientMessage
{
    public Answer Answer { get; set; }
    
    public ClientAnswerMessage(Answer answer)
    {
        Answer = answer;
    }
}