using System.Threading.Tasks;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.Entities;
using QuizServer.Models.SessionRelevant;

namespace QuizServer.Services.Interfaces;

public interface IHandleClientQuizJoinService
{
    public Task<Participant> HandleAsync(ClientJoinQuizMessage joinQuizMessage, ConnectedClient connectedClient);
}