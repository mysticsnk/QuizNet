using System.Threading.Tasks;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.Entities;

namespace QuizServer.Services.Interfaces;

public interface IHandleClientRegistrationService
{
    public Task HandleAsync(ClientRegistrationMessage message, ConnectedClient client);
}