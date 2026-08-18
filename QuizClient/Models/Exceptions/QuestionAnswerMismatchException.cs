using System;

namespace QuizClient.Models.Exceptions;

public class QuestionAnswerMismatchException : Exception
{
    public QuestionAnswerMismatchException() : base() { }

    public QuestionAnswerMismatchException(string message) : base(message) { }

    public QuestionAnswerMismatchException(string message, Exception innerException) 
        : base(message, innerException) { }
}