using QuizServer.Models.Interfaces;

namespace QuizServer.Models.Entities;

public class SpecificPortResolver : IPortResolver
{
    public string GetPort()
    {
        return "8080";
    }
}