using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.DatabaseRelevant.Entities;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Exceptions;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.Services;

public class UserRegistrationService : IUserRegistrationService
{
    public async Task VerifyAsync(string email, string passwordHash)
    {
        IUserRepository userRepository = Program.AppHost.Services.GetRequiredService<IUserRepository>();
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        
        UserAccount? foundUser =
            (await userRepository.GetAllUsersAsync()).FirstOrDefault(user => user.Email == email);

        if (foundUser == null) throw new UserNotFoundException();

        if (foundUser.PasswordHash != passwordHash)
        {
            throw new InvalidPasswordException();
        }
        serverState.Account = foundUser;
    }

    public async Task RegisterAsync(string userName, string email, string passwordHash)
    {
        IUserRepository userRepository = Program.AppHost.Services.GetRequiredService<IUserRepository>();
        
        UserAccount newUser = new UserAccount(userName, email, passwordHash);
        
        await userRepository.CreateAsync(newUser);
    }
}