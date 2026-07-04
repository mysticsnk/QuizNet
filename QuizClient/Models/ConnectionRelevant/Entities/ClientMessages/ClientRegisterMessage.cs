namespace QuizClient.Models.ConnectionRelevant.Entities.ClientMessages;

public class ClientRegisterMessage : ClientMessage
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public ClientRegisterMessage(string userName, string email, string passwordHash)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
    }
}