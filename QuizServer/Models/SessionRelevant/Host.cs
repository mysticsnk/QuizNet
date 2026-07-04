using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.Entities;
using QuizServer.Models.UserRelevant;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.SessionRelevant;

public class Host
{
    public Guid Id { get; set; }
    public UserAccount Account { get; set; }
    private SocketServer _server { get; set; }

    public Host(UserAccount account)
    {
        Account = account;
        Id = Guid.NewGuid();
        _server = Program.AppHost.Services.GetRequiredService<SocketServer>();
    }

    public void Start()
    {
        _server.Start();
    }

    public async Task AcceptClientAsync()
    {
        await _server.AcceptClientAsync();
    }
    
    public async Task<Answer> ReceiveAnswerAsync()
    {
        ConnectedClient client = _server.Clients[0];
        string answerJson = await _server.ReceiveMessageAsync(client);
        Answer answer = JsonSerializer.Deserialize<Answer>(answerJson)!;

        if (answer is SingleChoiceAnswer singleChoice)
        {
            Console.WriteLine("Received a single choice answer!");
        }
        else if (answer is MultiChoiceAnswer multiChoice)
        {
            Console.WriteLine("Received a multi choice answer!");
        }
        else if (answer is TrueFalseAnswer trueFalse)
        {
            Console.WriteLine("Received a true false answer!");
        }
        else if (answer is ShortTextAnswer shortText)
        {
            Console.WriteLine("Received a short text answer!");
        }
        else
        {
            Console.WriteLine("Unknown answer received");
        }

        return answer;
    }
    
}