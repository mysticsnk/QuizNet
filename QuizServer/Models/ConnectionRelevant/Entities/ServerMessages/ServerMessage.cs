using System.Text.Json.Serialization;

namespace QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(RegistrationResultMessage), "register")]
[JsonDerivedType(typeof(LoginResultMessage), "login")]
[JsonDerivedType(typeof(AnnouncementMessage), "announcement")]
[JsonDerivedType(typeof(KickMessage), "kick")]
[JsonDerivedType(typeof(QuestionMessage), "question")]
[JsonDerivedType(typeof(QuizJoinResultMessage), "joinResult")]
public abstract class ServerMessage
{
    
}