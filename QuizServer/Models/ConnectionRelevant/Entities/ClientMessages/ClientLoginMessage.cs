namespace QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;

public class ClientLoginMessage : ClientMessage
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public ClientLoginMessage(string userName, string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }
}