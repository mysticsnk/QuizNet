using QuizClient.Models.SessionRelevant;
using QuizClient.Models.SessionRelevant.Answers;

namespace QuizClient.Models.ConnectionRelevant.Entities.ClientMessages;

public class ClientAnswerMessage : ClientMessage
{
    public Participant Participant { get; set; }
    public Answer Answer { get; set; }

    public ClientAnswerMessage(Participant participant, Answer answer)
    {
        Participant = participant;
        Answer = answer;
    }
}