using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuizServer.Models.Entities;
using QuizServer.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace QuizServer;

public class SocketServer
{
    private HttpListener _serverSocket { get; set; }
    public List<ConnectedClient> Clients { get; set; } = new ();
    
    public SocketServer(IPortResolver resolver)
    {
        _serverSocket = new HttpListener();

        string port = resolver.GetPort();
        _serverSocket.Prefixes.Add($"http://localhost:{port}/");
    }
    
    public void Start()
    {
        _serverSocket.Start();
        
        Console.WriteLine("Server started!");
    }

    public async Task AcceptClientAsync()
    {
        HttpListenerContext context = await _serverSocket.GetContextAsync();
        HttpListenerRequest request = context.Request;
        
        if (!request.IsWebSocketRequest)
        {
            throw new WebSocketException("Invalid protocol, expected WS");
        }

        WebSocketContext wsContext = await context.AcceptWebSocketAsync(subProtocol: null);

        WebSocket ws = wsContext.WebSocket;
        string? userName = context.Request.QueryString["user"];
        
        Clients.Add(new ConnectedClient(ws, userName));
        Console.WriteLine("Server accepted a new client!");
    }

    public void Stop()
    {
        List<Task> tasks = new List<Task>();

        foreach (ConnectedClient client in Clients)
        {
            tasks.Add(client.Ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "OK", CancellationToken.None));
        }

        Task.WhenAll(tasks);
        
        Console.WriteLine("Server stopped!");
    }
    
    public async Task<string> ReceiveMessageAsync(ConnectedClient client)
    {
        WebSocket ws = client.Ws;
        
        using MemoryStream memoryStream = new MemoryStream();
        WebSocketReceiveResult result;

        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

        do
        {
            result = await ws.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                throw new WebSocketException("WS closed");
            }
            await memoryStream.WriteAsync(buffer.Array, 0, buffer.Count);
        } while (!result.EndOfMessage);

        string message = Encoding.ASCII.GetString(buffer.Array);
        Console.WriteLine($"Received the message {message} from a client");
        return message;
    }
    
    public async Task<bool> SendMessageAsync(ConnectedClient client, string message)
    {
        WebSocket ws = client.Ws;
        if (ws.State == WebSocketState.Closed) return false;
        
        Console.WriteLine($"Sent the message '{message}' to the client {client.UserName}");
        await SafeSendAsync(ws , message);
        return true;
    }

    public async Task BroadcastMessageAsync(string message)
    {
        List<Task> tasks = new List<Task>();
        
        foreach (ConnectedClient client in Clients)
        {
            tasks.Add(SendMessageAsync(client, message));
        }
    
        await Task.WhenAll(tasks);
        Console.WriteLine($"Broadcasted a message {message} to all clients");
    }
    
    private static async Task SafeSendAsync(WebSocket ws, string message)
    {
        try
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text,
                WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"Exception when sending: {ex.Message}");
        }
    }
}