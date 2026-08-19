using System.Threading.Tasks;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.Entities;

namespace QuizServer.Services.Interfaces;

public interface IHandleClientLoginService
{
    public Task HandleAsync(ClientLoginMessage loginMessage, ConnectedClient client);
}