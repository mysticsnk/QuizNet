using System;
using System.Collections.Generic;
using System.Linq;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.Exceptions;
using QuizServer.Models.QuizRelevant.Abstracts;
using CorrectAnswerAmountException = QuizServer.Models.Exceptions.CorrectAnswerAmountException;

namespace QuizServer.Models.QuizRelevant.Entities.Questions;

public class TrueFalseQuestion : Question
{

    public TrueFalseQuestion() : base()
    {
    }

    public TrueFalseQuestion(List<QuestionOption> options, string? title = null, byte[]? imageBytes = null, int pointsWeight = 100)
        : base(title, imageBytes, pointsWeight)
    {
        Options = options;

        if (options.Count != 2)
        {
            throw new InvalidOptionCountException($"Must be 2 options in a True/False question, got {options.Count}");
        }
        
        int correctOptionsCount = options.Count(op => op.IsCorrect);
        if (correctOptionsCount == 0)
        {
            throw new CorrectAnswerAmountException("No correct options");
        } 
        if (correctOptionsCount != 1)
        {
            throw new CorrectAnswerAmountException($"Expected 1 correct option. Got {correctOptionsCount}");
        }
    }
}