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
using QuizClient.Models.QuizRelevant.Abstracts;
using QuizClient.Models.Services.Interfaces;
using QuizClient.Models.SessionRelevant.Answers;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models;

public class SocketClient
{
    private ClientWebSocket _clientSocket { get; set; }
    public IPortResolver _portResolver { get; set; }
    public bool IsConnected { get; private set; } = false;
    
    private TaskCompletionSource<UserAccount>? _pendingRegistration { get; set; }
    private TaskCompletionSource<UserAccount>? _pendingLogin { get; set; }
    private TaskCompletionSource<QuizJoinResultMessage>? _pendingJoin { get; set; }
    private TaskCompletionSource<Question>? _pendingQuestion { get; set; }
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

    public async Task<UserAccount> RegisterAsync(string userName, string email, string passwordHash)
    {
        TaskCompletionSource<UserAccount> tcs = new();

        _pendingRegistration = tcs;
        
        ClientRegistrationMessage registrationMessage = new ClientRegistrationMessage(userName, email, passwordHash);
        
        await SendMessageAsync(registrationMessage);

        return await tcs.Task;
    }

    public async Task<UserAccount> LoginAsync(string userName, string email, string passwordHash)
    {
        TaskCompletionSource<UserAccount> tcs = new();

        _pendingLogin = tcs;

        ClientLoginMessage loginMessage = new ClientLoginMessage(userName, email, passwordHash);

        await SendMessageAsync(loginMessage);

        return await tcs.Task;
    }

    public async Task<QuizJoinResultMessage> JoinQuizAsync(string userName, string pin, UserAccount? account = null)
    {
        TaskCompletionSource<QuizJoinResultMessage> tcs = new();

        _pendingJoin = tcs;
        
        ClientJoinQuizMessage joinMessage = new ClientJoinQuizMessage(userName, pin, account);
        
        await SendMessageAsync(joinMessage);

        return await tcs.Task;
    }

    public async Task<Question> WaitForNextQuestionAsync()
    {
        TaskCompletionSource<Question> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
        
        _pendingQuestion = tcs;

        return await tcs.Task;
    }

    public async Task SendAnswerAsync(Answer answer)
    {
        ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
        
        ClientAnswerMessage message = new ClientAnswerMessage(clientState.CurrentSession.Participant, answer);

        await SendMessageAsync(message);
    }

    public async Task StartAcceptLoopAsync()
    {
        try
        {
            while (_clientSocket.State == WebSocketState.Open)
            {
                ServerMessage serverMessage = await ReceiveMessageAsync();

                if (serverMessage is RegistrationResultMessage registrationMessage)
                {
                    if (registrationMessage.IsSuccess)
                    {
                        ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
                        clientState.Account = registrationMessage.Account;
                        _pendingRegistration?.SetResult(registrationMessage.Account);
                        _pendingRegistration = null;
                    }
                    else
                    {
                        // TODO: Create a property for MVVM that shows these errors
                        Console.WriteLine(registrationMessage.Errors);
                    }
                }
                else if (serverMessage is LoginResultMessage loginMessage)
                {
                    if (loginMessage.IsSuccess)
                    {
                        ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
                        clientState.Account = loginMessage.Account;
                        _pendingLogin?.SetResult(loginMessage.Account);
                        _pendingLogin = null;
                    }
                    else
                    {
                        // TODO: Create a property for MVVM that shows these errors
                        Console.WriteLine(loginMessage.Errors);
                    }
                }
                else if (serverMessage is QuizJoinResultMessage joinResultMessage)
                {
                    if (joinResultMessage.IsSuccess)
                    {
                        IAnticheatApplyService anticheater =
                            Program.AppHost.Services.GetRequiredService<IAnticheatApplyService>();
                        anticheater.Apply();
                        
                        ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
                        clientState.CurrentSession = joinResultMessage.ClientQuizSession;
                         _pendingJoin?.SetResult(joinResultMessage);
                        _pendingJoin = null;
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
                    _pendingQuestion?.SetResult(questionMessage.Question);
                    _pendingQuestion = null;
                }
                else if (serverMessage is QuizEndedMessage quizEndedMessage)
                {
                    ClientState clientState = Program.AppHost.Services.GetRequiredService<ClientState>();
                    clientState.CurrentSession = null;
                    clientState.Result = quizEndedMessage.Result;
                    Console.WriteLine("Quiz ended");
                    Console.WriteLine($"Total score: {clientState.Result.TotalScore}");
                    Console.WriteLine($"Place: {clientState.Result.Place}");
                }
                else
                {
                    Console.WriteLine("Unknown message received");
                }
            }
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine("Server disconnected");
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