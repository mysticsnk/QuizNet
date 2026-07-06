using System;
using System.Threading.Tasks;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.Entities;
using QuizServer.Models.Services.Interfaces;

namespace QuizServer.Models.Services;

public class DummyHandleClientRegistrationService : IHandleClientRegistrationService
{
    public Task HandleAsync(ClientRegistrationMessage message, ConnectedClient client)
    {
        return new Task(() => Console.WriteLine($"Got message from {message.UserName}"));
    }
}