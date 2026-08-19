using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.DatabaseRelevant.Entities;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Exceptions;
using QuizServer.Models.UserRelevant;
using QuizServer.Services.Interfaces;

namespace QuizServer.Services;

public class UserRegistrationService : IUserRegistrationService
{
    public async Task VerifyAsync(string userName, string email, string passwordHash)
    {
        IUserRepository userRepository = Program.AppHost.Services.GetRequiredService<IUserRepository>();
        UserAccount? foundUser =
            (await userRepository.GetAllUsersAsync()).FirstOrDefault(user =>
                user.UserName == userName && user.Email == email);

        if (foundUser == null) throw new UserNotFoundException();

        if (foundUser.PasswordHash != passwordHash)
        {
            throw new InvalidPasswordException();
        }
    }

    public async Task RegisterAsync(string userName, string email, string passwordHash)
    {
        IUserRepository userRepository = Program.AppHost.Services.GetRequiredService<IUserRepository>();
        
        UserAccount newUser = new UserAccount(userName, email, passwordHash);
        
        await userRepository.CreateAsync(newUser);
    }
}