namespace QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;

public class KickMessage : ServerMessage
{
    public string Reason { get; set; } = string.Empty;

    public KickMessage(string reason)
    {
        Reason = reason;
    }
}