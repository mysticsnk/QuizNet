using System.Threading.Tasks;

namespace QuizServer.Models.Services.Interfaces;

public interface IUserRegistrationService
{
    public Task VerifyAsync(string email, string passwordHash);

    public Task RegisterAsync(string userName, string email, string passwordHash);
}