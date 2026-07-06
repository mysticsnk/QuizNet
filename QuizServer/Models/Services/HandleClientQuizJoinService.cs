using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.Entities;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.SessionRelevant;

namespace QuizServer.Models.Services;

public class HandleClientQuizJoinService : IHandleClientQuizJoinService
{
    public async Task HandleAsync(ClientJoinQuizMessage joinQuizMessage, ConnectedClient client)
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        SocketServer server = Program.AppHost.Services.GetRequiredService<SocketServer>();
        QuizJoinResultMessage resultMessage = new QuizJoinResultMessage();
        
        if (serverState.CurrentSession == null)
        {
            resultMessage.IsSuccess = false;
            resultMessage.AddError("No sessions currently active");
            await server.SendMessageAsync(client, resultMessage);
        }

        if (!serverState.CurrentSession.IsCorrectPin(joinQuizMessage.Pin))
        {
            resultMessage.IsSuccess = false;
            resultMessage.AddError("Invalid pin");
            await server.SendMessageAsync(client, resultMessage);
        }

        resultMessage.IsSuccess = true;
        
        Participant newParticipant = new Participant(joinQuizMessage.UserName, joinQuizMessage.Account);
        resultMessage.Participant = newParticipant;
        
        ClientQuizSession clientSession = new ClientQuizSession();
        clientSession.Participant = newParticipant;
        clientSession.Quiz = serverState.CurrentSession.Quiz;
        clientSession.CurrentQuestion = serverState.CurrentSession.CurrentQuestion;
        resultMessage.ClientQuizSession = clientSession;

        await server.SendMessageAsync(client, resultMessage);

    }
}