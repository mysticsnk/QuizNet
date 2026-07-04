using System.Text.Json.Serialization;

namespace QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ClientRegisterMessage), "register")]
[JsonDerivedType(typeof(ClientLoginMessage), "login")]
[JsonDerivedType(typeof(ClientAnswerMessage), "answer")]
public abstract class ClientMessage
{
    
}