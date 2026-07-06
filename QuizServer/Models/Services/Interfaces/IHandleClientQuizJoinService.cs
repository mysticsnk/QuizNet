using System.Threading.Tasks;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.Entities;

namespace QuizServer.Models.Services.Interfaces;

public interface IHandleClientQuizJoinService
{
    public Task HandleAsync(ClientJoinQuizMessage joinQuizMessage, ConnectedClient connectedClient);
}