namespace QuizClient.Services.Interfaces;

public interface IPasswordHashingService
{
    public string Hash(string text);
    public bool Verify(string hash, string password);
}