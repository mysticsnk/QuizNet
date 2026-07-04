namespace QuizClient.Models.ConnectionRelevant.Entities.ServerMessages;

public class AnnouncementMessage : ServerMessage
{
    public string Text { get; set; } = string.Empty;

    public AnnouncementMessage(string text)
    {
        Text = text;
    }
}