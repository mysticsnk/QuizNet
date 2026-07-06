using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.Entities;
using QuizServer.Models.Exceptions;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.Services;

public class HandleClientLoginService : IHandleClientLoginService
{
    public async Task HandleAsync(ClientLoginMessage loginMessage, ConnectedClient client)
    {
        IUserRegistrationService registrationService = 
            Program.AppHost.Services.GetRequiredService<IUserRegistrationService>();
        SocketServer server = Program.AppHost.Services.GetRequiredService<SocketServer>();
        
        LoginResultMessage serverMessage = new LoginResultMessage();


        try
        {
            await registrationService.VerifyAsync(loginMessage.UserName, loginMessage.Email,
                loginMessage.PasswordHash);
        }
        catch (UserNotFoundException)
        {
            serverMessage.IsSuccess = false;
            serverMessage.AddError("User wasn't found");
            await server.SendMessageAsync(client, serverMessage);
        }
        catch (InvalidPasswordException)
        {
            serverMessage.IsSuccess = false;
            serverMessage.AddError("Invalid password");
            await server.SendMessageAsync(client, serverMessage);
        }

        UserAccount account =
            new UserAccount(loginMessage.UserName, loginMessage.Email, loginMessage.PasswordHash);
        await server.SendMessageAsync(client, serverMessage);
    }
}