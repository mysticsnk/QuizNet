using System.Text.Json.Serialization;

namespace QuizClient.Models.ConnectionRelevant.Entities.ServerMessages;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(AccountMessage), "account")]
[JsonDerivedType(typeof(AnnouncementMessage), "announcement")]
[JsonDerivedType(typeof(KickMessage), "kick")]
[JsonDerivedType(typeof(QuestionMessage), "question")]
public abstract class ServerMessage
{
    
}