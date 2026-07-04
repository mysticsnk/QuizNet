using System.Net.WebSockets;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.Entities;

public class ConnectedClient
{
    public WebSocket Ws { get; set; }
    public UserAccount? Account { get; set; }

    public ConnectedClient(WebSocket ws)
    {
        Ws = ws;
    }
    
    public ConnectedClient(WebSocket ws, UserAccount? account)
    {
        Ws = ws;
        Account = account;
    }
}