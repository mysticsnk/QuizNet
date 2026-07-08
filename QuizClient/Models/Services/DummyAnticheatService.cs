using System;
using QuizClient.Models.Services.Interfaces;

namespace QuizClient.Models.Services;

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