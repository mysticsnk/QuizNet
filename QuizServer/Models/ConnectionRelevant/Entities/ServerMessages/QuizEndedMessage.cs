using QuizServer.Models.Entities.QuizRelevant;

namespace QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;

public class QuizEndedMessage : ServerMessage
{
    public ParticipantResult Result { get; set; }

    public QuizEndedMessage(ParticipantResult result)
    {
        Result = result;
    }
}