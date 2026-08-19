using System;
using System.Globalization;
using System.Threading.Tasks;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.Entities;
using QuizServer.Services.Interfaces;

namespace QuizServer.Services;
public class DummyHandleClientLoginService : IHandleClientLoginService
{
    public Task HandleAsync(ClientLoginMessage loginMessage, ConnectedClient client)
    {
        Console.WriteLine($"Received a login message from {loginMessage.UserName}");
        return Task.CompletedTask;
    }
}