using QuizServer.Services.Interfaces;

namespace QuizServer.Services;

public class DummyPortResolverService : IPortResolverService
{
    public string GetPort()
    {
        return "6969";
    }
}