using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.Entities;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.Services;

public class HandleClientRegistrationService : IHandleClientRegistrationService
{
    public IUserRegistrationService RegistrationService { get; set; }
    
    public HandleClientRegistrationService(IUserRegistrationService registrationService)
    {
        RegistrationService = registrationService;
    }
    
    public async Task HandleAsync(ClientRegisterMessage registerMessage, ConnectedClient client)
    {
        IUserRegistrationService registrationService =
            Program.AppHost.Services.GetRequiredService<IUserRegistrationService>();

        UserAccount account = new UserAccount(registerMessage.UserName, registerMessage.Email,
            registerMessage.PasswordHash);
                
        await registrationService.RegisterAsync(account.UserName, account.Email, account.PasswordHash);
                
        AccountMessage serverMessage = new AccountMessage(account);
        SocketServer server = Program.AppHost.Services.GetRequiredService<SocketServer>();
        await server.SendMessageAsync(client, serverMessage);
    }
}