using System;

namespace QuizClient.Models.Exceptions;

public class InvalidOptionCountException : Exception
{
    public InvalidOptionCountException() : base() { }

    public InvalidOptionCountException(string message) : base(message) { }

    public InvalidOptionCountException(string message, Exception innerException) 
        : base(message, innerException) { }
}