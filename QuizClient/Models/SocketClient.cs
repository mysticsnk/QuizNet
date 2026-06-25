using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuizClient.Models;

public class SocketClient
{
    private HttpClient _client { get; set; }

    public SocketClient()
    {
        _client = new HttpClient();
        
        Console.WriteLine("Client created");
    }

    public async Task<HttpResponseMessage> SendMessageAsync(string message)
    {
        HttpRequestMessage request = new HttpRequestMessage();
        request.Content = new StringContent(message, Encoding.ASCII);
        request.Method = new HttpMethod("POST");
        request.RequestUri = new Uri("http://localhost:6969/");
        Console.WriteLine($"Sent the message '{message}' to the server");
        HttpResponseMessage responseMessage = await _client.SendAsync(request);
        return responseMessage;
    }

    public async Task<string> GetResponseContent(HttpResponseMessage responseMessage)
    {
        string message = await responseMessage.Content.ReadAsStringAsync();
        return message;
    }
    
    
}