using System.Net.WebSockets;

namespace QuizServer.Models.Entities;

public class ConnectedClient
{
    public WebSocket Ws { get; set; }
    public string UserName { get; set; } 

    public ConnectedClient(WebSocket ws, string? userName)
    {
        Ws = ws;
        UserName = userName ?? string.Empty;
    }
}