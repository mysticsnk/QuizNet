using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.DatabaseRelevant.Interfaces;
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
        IUserRepository userRepository = Program.AppHost.Services.GetRequiredService<IUserRepository>();
        
        LoginResultMessage serverMessage = new LoginResultMessage();


        try
        {
            await registrationService.VerifyAsync(loginMessage.Email,
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

        string userName = (await userRepository.GetAllUsersAsync())
            .FirstOrDefault(u => u.Email == loginMessage.Email && u.PasswordHash == loginMessage.PasswordHash)
            ?.UserName;
        UserAccount account =
            new UserAccount(userName, loginMessage.Email, loginMessage.PasswordHash);
        await server.SendMessageAsync(client, serverMessage);
    }
}