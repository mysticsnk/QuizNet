using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.SessionRelevant;

namespace QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;

public class QuestionMessage : ServerMessage
{
    public Question Question { get; set; }

    public QuestionMessage(Question question)
    {
        Question = question;
    }
}