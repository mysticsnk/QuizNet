using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizClient.Models;
using QuizServer.Models;
using QuizServer.Models.SessionRelevant.Answers;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.SessionRelevant;

public class Participant
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public int Points { get; set; }
    public UserAccount? Account { get; set; }

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

    }

    /*public async Task<bool> SendAnswerAsync(Answer answer)
    {
        if (!_client.IsConnected)
        {
            await _client.ConnectToServerAsync();
        }
        string answerJson = JsonSerializer.Serialize(answer);
        
        bool isSent = await _client.SendMessageAsync(answerJson);

        return isSent;
    }*/
}