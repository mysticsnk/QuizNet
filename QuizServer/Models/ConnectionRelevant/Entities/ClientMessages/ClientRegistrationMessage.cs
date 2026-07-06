namespace QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;

public class ClientRegistrationMessage : ClientMessage
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public ClientRegistrationMessage(string userName, string email, string passwordHash)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
    }
}