using QuizClient.Services.Interfaces;

namespace QuizClient.Services;

public class DummyPortResolver : IPortResolver
{
    public string GetPort()
    {
        return "6969";
    }
}