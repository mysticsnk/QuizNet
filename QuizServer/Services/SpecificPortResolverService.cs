using QuizServer.Services.Interfaces;

namespace QuizServer.Services;

public class SpecificPortResolverService : IPortResolverService
{
    public string GetPort()
    {
        return "8080";
    }
}