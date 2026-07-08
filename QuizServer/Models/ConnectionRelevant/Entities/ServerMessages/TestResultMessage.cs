namespace QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;

public class TestResultMessage : ServerMessage
{
    public int Place { get; set; }
    public int Points { get; set; }

    public TestResultMessage(int place, int points)
    {
        Place = place;
        Points = points;
    }
}