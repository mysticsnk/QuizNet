using System.Text.Json.Serialization;

namespace QuizServer.Models.ConnectionRelevant.Entities.ClientMessages;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ClientRegistrationMessage), "register")]
[JsonDerivedType(typeof(ClientLoginMessage), "login")]
[JsonDerivedType(typeof(ClientAnswerMessage), "answer")]
[JsonDerivedType(typeof(ClientJoinQuizMessage), "joinQuiz")]
public abstract class ClientMessage
{
    
}