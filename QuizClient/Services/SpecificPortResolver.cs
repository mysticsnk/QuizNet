using QuizClient.Services.Interfaces;

namespace QuizClient.Services;

public class SpecificPortResolver : IPortResolver
{
    public string GetPort()
    {
        return "8080";
    }
}