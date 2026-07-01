using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizClient.Models.DatabaseRelevant.Entities;
using QuizClient.Models.DatabaseRelevant.Interfaces;
using QuizClient.Models.Exceptions;
using QuizClient.Models.Services.Interfaces;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.Services;

public class UserRegistrationService : IUserRegistrationService
{
    public async Task VerifyAsync(string userName, string email, string password)
    {
        IUserRepository userRepository = Program.AppHost.Services.GetRequiredService<IUserRepository>();
        UserAccount? foundUser =
            (await userRepository.GetAllUsersAsync()).FirstOrDefault(user =>
                user.UserName == userName && user.Email == email);

        if (foundUser == null) throw new UserNotFoundException();

        IPasswordHashingService hasher = Program.AppHost.Services.GetRequiredService<IPasswordHashingService>();
        if (!hasher.Verify(foundUser.PasswordHash, password))
        {
            throw new InvalidPasswordException();
        }
    }

    public async Task RegisterAsync(string userName, string email, string password)
    {
        IUserRepository userRepository = Program.AppHost.Services.GetRequiredService<IUserRepository>();
        IPasswordHashingService hasher = Program.AppHost.Services.GetRequiredService<IPasswordHashingService>();
        
        string passwordHash = hasher.Hash(password);
        UserAccount newUser = new UserAccount(userName, email, passwordHash);
        
        await userRepository.CreateAsync(newUser);
    }
}