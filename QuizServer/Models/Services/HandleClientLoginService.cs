using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.Entities;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.Services;

public class HandleClientLoginService : IHandleClientLoginService
{
    public async Task HandleAsync(ClientLoginMessage loginMessage, ConnectedClient client)
    {
        IUserRegistrationService registrationService = 
            Program.AppHost.Services.GetRequiredService<IUserRegistrationService>();

        await registrationService.VerifyAsync(loginMessage.UserName, loginMessage.Email,
            loginMessage.PasswordHash);

        UserAccount account =
            new UserAccount(loginMessage.UserName, loginMessage.Email, loginMessage.PasswordHash);
        AccountMessage serverMessage = new AccountMessage(account);
        SocketServer server = Program.AppHost.Services.GetRequiredService<SocketServer>();
        await server.SendMessageAsync(client, serverMessage);
    }
}