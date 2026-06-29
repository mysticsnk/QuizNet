using QuizClient.Models.Interfaces;

namespace QuizClient.Models.Entities;

public class DummyPortResolver : IPortResolver
{
    public string GetPort()
    {
        return "6969";
    }
}