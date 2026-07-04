using QuizClient.Models.QuizRelevant.Abstracts;

namespace QuizClient.Models.ConnectionRelevant.Entities.ServerMessages;

public class QuestionMessage : ServerMessage
{
    public Question Question { get; set; }

    public QuestionMessage(Question question)
    {
        Question = question;
    }
}