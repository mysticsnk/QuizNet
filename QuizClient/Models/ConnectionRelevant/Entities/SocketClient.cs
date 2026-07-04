using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizClient.Models.Interfaces;

namespace QuizClient.Models;

public class SocketClient
{
    private ClientWebSocket _clientSocket { get; set; }
    public string UserName { get; set; }
    public IPortResolver _portResolver { get; set; }
    public bool IsConnected { get; private set; } = false;

    public SocketClient(string userName, IPortResolver portResolver)
    {
        UserName = userName;
        _portResolver = portResolver;
        _clientSocket = new ClientWebSocket();
        
        Console.WriteLine("Client created!");
    }

    public async Task ConnectToServerAsync()
    {
        string port = _portResolver.GetPort();
        Uri serverUri = new Uri($"ws://localhost:{port}/ws?user={UserName}");
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

    public async Task<bool> SendMessageAsync(string message)
    {
        if (_clientSocket.State == WebSocketState.Closed) return false;

        Console.WriteLine($"Sent the message '{message}' to the server");
        await SafeSendAsync(_clientSocket , message);
        return true;
    }

    public async Task<string> ReceiveMessageAsync()
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

        string message = Encoding.UTF8.GetString(memoryStream.ToArray());
        Console.WriteLine($"Received the message {message} from the server");

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