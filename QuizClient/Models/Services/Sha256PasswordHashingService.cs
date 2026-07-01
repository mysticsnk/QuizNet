using System.Security.Cryptography;
using System.Text;
using QuizClient.Models.Services.Interfaces;

namespace QuizClient.Models.Services;

public class Sha256PasswordHashingService : IPasswordHashingService
{
    public string Hash(string text)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(text);
        byte[] hashedBytes = SHA256.HashData(bytes);
        return Encoding.ASCII.GetString(hashedBytes);
    }

    public bool Verify(string hash, string password)
    {
        return hash == Hash(password);
    }
}