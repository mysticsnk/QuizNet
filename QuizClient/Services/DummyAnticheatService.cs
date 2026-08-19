using System;
using QuizClient.Services.Interfaces;

namespace QuizClient.Services;

public class DummyAnticheatService : IAnticheatApplyService
{
    public void Apply()
    {
        Console.WriteLine("Anticheat applied!");
    }

    public void Disable()
    {
        Console.WriteLine("Anticheat disabled!");
    }
}