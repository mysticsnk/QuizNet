using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.Entities;
using QuizServer.Models.UserRelevant;
using QuizServer.Services.Interfaces;

namespace QuizServer.Services;

public class HandleClientRegistrationService : IHandleClientRegistrationService
{
    public IUserRegistrationService RegistrationService { get; set; }
    
    public HandleClientRegistrationService(IUserRegistrationService registrationService)
    {
        RegistrationService = registrationService;
    }
    
    public async Task HandleAsync(ClientRegistrationMessage registrationMessage, ConnectedClient client)
    {
        IUserRegistrationService registrationService =
            Program.AppHost.Services.GetRequiredService<IUserRegistrationService>();

        UserAccount account = new UserAccount(registrationMessage.UserName, registrationMessage.Email,
            registrationMessage.PasswordHash);
                
        await registrationService.RegisterAsync(account.UserName, account.Email, account.PasswordHash);
                
        RegistrationResultMessage resultMessage = new RegistrationResultMessage(true, account);
        SocketServer server = Program.AppHost.Services.GetRequiredService<SocketServer>();
        await server.SendMessageAsync(client, resultMessage);
    }
}