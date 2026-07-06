using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizClient.Models.AppRelevant;
using QuizClient.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizClient.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizClient.Models.Interfaces;

namespace QuizClient.Models;

public class SocketClient
{
    private ClientWebSocket _clientSocket { get; set; }
    public IPortResolver _portResolver { get; set; }
    public bool IsConnected { get; private set; } = false;

    public SocketClient(IPortResolver portResolver)
    {
        _portResolver = portResolver;
        _clientSocket = new ClientWebSocket();
        
        Console.WriteLine("Client created!");
    }

    public async Task ConnectToServerAsync()
    {
        string port = _portResolver.GetPort();
        Uri serverUri = new Uri($"ws://localhost:{port}");
        try
        {
            await _clientSocket.ConnectAsync(serverUri, CancellationToken.None);
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"Failed to connect to a server: {ex.Message}");
        }

        IsConnected = true;
    }

    public async Task StartAcceptLoopAsync()
    {
        try
        {
            while (_clientSocket.State == WebSocketState.Open)
            {
                ServerMessage serverMessage = await ReceiveMessageAsync();

                if (serverMessage is AccountResultMessage accountMessage)
                {
                    if (accountMessage.IsSuccess)
                    {
                        ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
                        clientState.Account = accountMessage.Account;
                    }
                    else
                    {
                        // TODO: Create a property for MVVM that shows these errors
                        Console.WriteLine(accountMessage.Errors);
                    }
                }
                else if (serverMessage is QuizJoinResultMessage joinResultMessage)
                {
                    if (joinResultMessage.IsSuccess)
                    {
                        ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
                        clientState.CurrentSession = joinResultMessage.ClientQuizSession;
                    }
                    else
                    {
                        // TODO: Create a property for MVVM that shows these errors
                        Console.WriteLine(joinResultMessage.Errors);
                    }
                }
                else if (serverMessage is AnnouncementMessage announcementMessage)
                {
                    // TODO: Create a property for MVVM that shows this popup
                    Console.WriteLine(announcementMessage.Text);
                }
                else if (serverMessage is KickMessage kickMessage)
                {
                    // TODO: Create a property for MVVM that shows this popup
                    Console.WriteLine($"You were kicked. Reason {kickMessage.Reason}");
                    ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
                    clientState.CurrentSession = null;
                }
                else if (serverMessage is QuestionMessage questionMessage)
                {
                    ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
                    clientState.CurrentSession.CurrentQuestion = questionMessage.Question;
                }
                else
                {
                    Console.WriteLine("Unknown message received");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("INNER:");
                Console.WriteLine(ex.InnerException);
            }

            Console.WriteLine(ex.StackTrace);
        }
    }

    public async Task<bool> SendMessageAsync(ClientMessage message)
    {
        if (_clientSocket.State == WebSocketState.Closed) return false;

        string messageJson = JsonSerializer.Serialize(message);
        await SafeSendAsync(_clientSocket , messageJson);
        return true;
    }

    public async Task<ServerMessage> ReceiveMessageAsync()
    {
        using MemoryStream memoryStream = new MemoryStream();
        WebSocketReceiveResult result;

        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

        do
        {
            result = await _clientSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                throw new WebSocketException("WS closed");
            }
            await memoryStream.WriteAsync(buffer.Array, 0, result.Count);
        } while (!result.EndOfMessage);

        string messageJson = Encoding.UTF8.GetString(memoryStream.ToArray());
        ServerMessage message = JsonSerializer.Deserialize<ServerMessage>(messageJson);

        return message;
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

    public async void DisconnectFromServerAsync()
    {
        await _clientSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal closure",
            CancellationToken.None);
        IsConnected = false;
    }
    
    
}