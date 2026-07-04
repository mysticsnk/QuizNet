namespace QuizServer.Models.Services.Interfaces;

public interface IUserValidationService
{
    public bool IsValidEmail(string email);
    public bool IsValidPassword(string password);
}