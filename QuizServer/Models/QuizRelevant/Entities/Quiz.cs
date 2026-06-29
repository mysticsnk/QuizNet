using System.Collections.Generic;
using System.IO;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.QuizRelevant.Entities.Questions;

namespace QuizServer.Models.Entities.QuizRelevant;

public class  Quiz
{
    public List<Question> Questions { get; set; } = new();

    public Quiz(List<Question> questions)
    {
        Questions = questions;
    }

    public Quiz(string jsonFilePath)
    {
        string quizJson = File.ReadAllText(jsonFilePath);
        Questions.Add(new SingleChoiceQuestion(new List<QuestionOption>
        {
            new QuestionOption(true, "Hello frogs1", null),
            new QuestionOption(false, "Hello frogs2", null),
            new QuestionOption(false, "Hello frogs3", null),
            new QuestionOption(false, "Hello frogs4", null) 
        }));
    }

    public void AddQuestion(Question question)
    {
        Questions.Add(question);
    }

    public void DeleteQuestion(Question question)
    {
        Questions.Remove(question);
    }
    
    
}