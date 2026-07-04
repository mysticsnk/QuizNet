using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizClient.Models.Interfaces;
using QuizClient.Models.SessionRelevant.Answers;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.SessionRelevant;

public class Participant
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public int Points { get; set; }
    public UserAccount? Account { get; set; }
    public SocketClient _socketClient { get; set; }

    public Participant(string userName, UserAccount? account)
    {
        UserName = userName;
        if (account != null)
        {
            Account = account;
            Id = Account.Id;
        }
        else
        {
            Id = Guid.NewGuid();
        }

        _socketClient = Program.AppHost.Services.GetRequiredService<SocketClient>();
    }

    /*public async Task<bool> SendAnswerAsync(Answer answer)
    {
        if (!_socketClient.IsConnected)
        {
            await _socketClient.ConnectToServerAsync();
        }
        string answerJson = JsonSerializer.Serialize(answer);
        
        bool isSent = await _socketClient.SendMessageAsync(answerJson);

        return isSent;
    }*/
}