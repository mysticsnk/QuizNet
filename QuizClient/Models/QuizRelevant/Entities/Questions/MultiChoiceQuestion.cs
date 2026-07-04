using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;
using QuizClient.Models.Entities.QuizRelevant;
using QuizClient.Models.Exceptions;
using QuizClient.Models.QuizRelevant.Abstracts;

namespace QuizClient.Models.QuizRelevant.Entities.Questions;

public class MultiChoiceQuestion : Question
{
    public List<QuestionOption> Options { get; set; } = new ();
    
    public MultiChoiceQuestion() : base()
    {
        
    }

    public MultiChoiceQuestion(List<QuestionOption> options, string? title = null, byte[]? imageBytes = null, int pointsWeight = 100) 
    :base(title, imageBytes, pointsWeight)
    {
        int correctOptionsCount = options.Count(op => op.IsCorrect);
        if (correctOptionsCount == 0)
        {
            throw new CorrectAnswerAmountException("No correct options");
        }
        
        Options = options;
    }

    public void AddOption(QuestionOption option)
    {
        Options.Add(option);
    }

    public void DeleteOption(QuestionOption option)
    {
        Options.Remove(option);
    }
    
    
}