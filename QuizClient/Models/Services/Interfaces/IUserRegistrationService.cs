using System.Threading.Tasks;

namespace QuizClient.Models.Services.Interfaces;

public interface IUserRegistrationService
{
    public Task VerifyAsync(string userName, string email, string password);

    public Task RegisterAsync(string userName, string email, string password);
}