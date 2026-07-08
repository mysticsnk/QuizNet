using System.Net.WebSockets;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.Entities;

public class ConnectedClient
{
    public WebSocket Ws { get; set; }
    public Participant? Participant { get; set; }

    public ConnectedClient(WebSocket ws)
    {
        Ws = ws;
    }
}