using QuizClient.Models.Interfaces;

namespace QuizClient.Models.Entities;

public class SpecificPortResolver : IPortResolver
{
    public string GetPort()
    {
        return "8080";
    }
}