using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using QuizServer.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.SessionRelevant;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.UserRelevant;
using QuizServer.Services.Interfaces;
using ClientAnswerMessage = QuizServer.Models.ConnectionRelevant.Entities.ClientMessages.ClientAnswerMessage;
using ClientLoginMessage = QuizServer.Models.ConnectionRelevant.Entities.ClientMessages.ClientLoginMessage;
using ClientMessage = QuizServer.Models.ConnectionRelevant.Entities.ClientMessages.ClientMessage;

namespace QuizServer;

public class SocketServer
{
    private HttpListener _serverSocket { get; set; }
    public List<ConnectedClient> Clients { get; set; } = new ();
    
    public SocketServer(IPortResolverService resolverService)
    {
        _serverSocket = new HttpListener();

        string port = resolverService.GetPort();
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
        Clients.Add(client);

        _ = Task.Run(() => HandleClient(client));
    }

    private async Task HandleClient(ConnectedClient client)
    {
        try
        {
            while (client.Ws.State == WebSocketState.Open)
            {
                ClientMessage clientMessage = await ReceiveClientMessageAsync(client);
                if (clientMessage is ClientRegistrationMessage registrationMessage)
                {
                    IHandleClientRegistrationService registrationService =
                        Program.AppHost.Services.GetRequiredService<IHandleClientRegistrationService>();
                    
                    await registrationService.HandleAsync(registrationMessage, client);
                }
                else if (clientMessage is ClientLoginMessage loginMessage)
                {
                    IHandleClientLoginService loginService =
                        Program.AppHost.Services.GetRequiredService<IHandleClientLoginService>();

                    await loginService.HandleAsync(loginMessage, client);
                }
                else if (clientMessage is ClientJoinQuizMessage joinQuizMessage)
                {
                    IHandleClientQuizJoinService quizJoinService =
                        Program.AppHost.Services.GetRequiredService<IHandleClientQuizJoinService>();
                    
                    client.Participant = await quizJoinService.HandleAsync(joinQuizMessage, client);
                }
                else if (clientMessage is ClientAnswerMessage answerMessage)
                {
                    ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();

                    IQuizMode? mode = serverState?.CurrentSession?.Mode;

                    if (mode == null)
                    {
                        Console.WriteLine("The mode is not initialized");
                        continue;
                    }

                    await mode.HandleAnswerAsync(client.Participant, answerMessage.Answer);
                }
                else
                {

                }
            }
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"Client {client.Participant.UserName} disconnected");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                Console.WriteLine("INNER:");
                Console.WriteLine(ex.InnerException);
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
        
        await SafeSendAsync(ws, jsonMessage);
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

    public async Task BroadcastToParticipantsAsync(ServerMessage message)
    {
        List<Task> tasks = new List<Task>();
        
        foreach (ConnectedClient client in Clients)
        {
            if (client.Participant != null) 
                tasks.Add(SendMessageAsync(client, message));
        }
    
        await Task.WhenAll(tasks);
        Console.WriteLine($"Broadcasted a message {message} to all clients");
    }

    public async Task KickParticipantAsync(Participant participant, string reason)
    {
        KickMessage message = new KickMessage(reason);

        ConnectedClient? client = Clients.Where(c => c.Participant.Id == participant.Id).FirstOrDefault();

        if (client == null)
        {
            Console.WriteLine("Participant for kicking not found");
            return;
        }

        client.Participant = null;
        
        await SendMessageAsync(client, message);

        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        serverState.CurrentSession.KickParticipant(participant);
    }

    public async Task BroadcastAnnouncementAsync(string announcement)
    {
        AnnouncementMessage message = new AnnouncementMessage(announcement);

        await BroadcastMessageAsync(message);
    }

    public async Task BroadcastAnnouncementToParticipantsAsync(string announcement)
    {
        AnnouncementMessage message = new AnnouncementMessage(announcement);
        
        await BroadcastToParticipantsAsync(message);
    }

    public async Task SendQuestionAsync(Participant participant, Question question)
    {
        QuestionMessage message = new QuestionMessage(question);
        ConnectedClient client = Clients.Where(c => c.Participant.Id == participant.Id).FirstOrDefault();
        await SendMessageAsync(client, message);
    }

    public async Task BroadcastQuestionAsync(Question question)
    {
        foreach (ConnectedClient client in Clients)
        {
            List<Task> tasks = new List<Task>();
            
            if (client.Participant != null)
            {
                tasks.Add(SendQuestionAsync(client.Participant, question));
            }

            await Task.WhenAll(tasks);
        }
    }

    public async Task SendQuizEndMessageAsync(Participant participant)
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        
        QuizEndedMessage message = new QuizEndedMessage(serverState.ParticipantResults.FirstOrDefault(pr => pr.Participant.Id == participant.Id));
        ConnectedClient client = Clients.Where(c => c.Participant.Id == participant.Id).FirstOrDefault();
        await SendMessageAsync(client, message);
    }

    public async Task BroadcastQuizEndMessageAsync()
    {
        foreach (ConnectedClient client in Clients)
        {
            List<Task> tasks = new List<Task>();
            
            if (client.Participant != null)
            {
                tasks.Add(SendQuizEndMessageAsync(client.Participant));
            }

            await Task.WhenAll(tasks);
        }
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