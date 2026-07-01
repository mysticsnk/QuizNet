using System;

namespace QuizClient.Models.Exceptions;

public class CorrectAnswerAmountException : Exception
{
    public CorrectAnswerAmountException() : base() { }

    public CorrectAnswerAmountException(string message) : base(message) { }

    public CorrectAnswerAmountException(string message, Exception innerException) 
        : base(message, innerException) { }
}