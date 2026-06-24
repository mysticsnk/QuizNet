using QuizServer.Models.Interfaces;

namespace QuizServer.Models.Entities;

public class DummyPortResolver : IPortResolver
{
    public string GetPort()
    {
        return "6969";
    }
}