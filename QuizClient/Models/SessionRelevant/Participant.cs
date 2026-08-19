using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizClient.Models.SessionRelevant.Answers;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.SessionRelevant;

public class Participant
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public int Points { get; set; }
    public UserAccount? Account { get; set; }

    public Participant(string userName, UserAccount? account = null)
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
}