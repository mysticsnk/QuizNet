using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuizClient.Models;

public class SocketClient
{
    private TcpClient _client { get; set; }

    public SocketClient()
    {
        _client = new TcpClient("127.0.0.1", 6969);
        Console.WriteLine("Client created");
    }

    public void Close()
    {
        _client.Close();
        Console.WriteLine("Client closed");
    }

    public async Task<string> ReadMessageAsync()
    {
        NetworkStream stream = _client.GetStream();
        byte[] bytes = new byte[1024];
        await stream.ReadAsync(bytes, 0, bytes.Length);
        string message = Encoding.ASCII.GetString(bytes);
        Console.WriteLine($"Read message: {message}");
        return message;
    }
}