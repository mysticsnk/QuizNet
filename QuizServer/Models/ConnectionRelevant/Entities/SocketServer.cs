using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using QuizServer.Models.Entities;
using QuizServer.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.UserRelevant;

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
    
    public async Task StartAsync()
    {
        _serverSocket.Start();
        
        Console.WriteLine("Server started!");

        while (true)
        {
            await AcceptClientAsync();
        }
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

        ConnectedClient client = new ConnectedClient(wsContext.WebSocket);
        
        Console.WriteLine("Server accepted a new client!");

        _ = Task.Run(() => HandleClient(client));
    }

    private async Task HandleClient(ConnectedClient client)
    {
        while (client.Ws.State == WebSocketState.Open)
        {
            ClientMessage clientMessage = await ReceiveClientMessageAsync(client);
            if (clientMessage is ClientRegisterMessage registerMessage)
            {
                IHandleClientRegistrationService registrationService =
                    Program.AppHost.Services.GetRequiredService<IHandleClientRegistrationService>();

                await registrationService.HandleAsync(registerMessage, client);
            }
            else if (clientMessage is ClientLoginMessage loginMessage)
            {
                IHandleClientLoginService loginService =
                    Program.AppHost.Services.GetRequiredService<IHandleClientLoginService>();
                
                await loginService.HandleAsync(loginMessage, client);
            }
            else if (clientMessage is ClientAnswerMessage answerMessage)
            {
                
            }
            else
            {
                
            }
        }
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
    
    public async Task<ClientMessage> ReceiveClientMessageAsync(ConnectedClient client)
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
            await memoryStream.WriteAsync(buffer.Array, 0, result.Count);
        } while (!result.EndOfMessage);

        string messageText = Encoding.UTF8.GetString(memoryStream.ToArray());
        ClientMessage message = JsonSerializer.Deserialize<ClientMessage>(messageText);
        return message;
    }
    
    public async Task<bool> SendMessageAsync(ConnectedClient client, ServerMessage message)
    {
        WebSocket ws = client.Ws;
        if (ws.State == WebSocketState.Closed) return false;

        string jsonMessage = JsonSerializer.Serialize(message);
        
        await SafeSendAsync(ws ,jsonMessage);
        return true;
    }

    public async Task BroadcastMessageAsync(ServerMessage message)
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
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text,
                WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"Exception when sending: {ex.Message}");
        }
    }
}