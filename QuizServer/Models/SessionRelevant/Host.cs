using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.Entities;
using QuizServer.Models.UserRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.SessionRelevant;

public class Host
{
    public Guid Id { get; set; }
    public UserAccount Account { get; set; }
    private SocketServer _server { get; set; }

    public Host(UserAccount account)
    {
        Account = account;
        Id = Guid.NewGuid();
        _server = Program.AppHost.Services.GetRequiredService<SocketServer>();
    }

    public async Task BeginAcceptingClients()
    {
        await _server.StartAsync();
    }

}