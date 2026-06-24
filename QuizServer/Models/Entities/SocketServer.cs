using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuizServer.Models.Entities;

public class SocketServer
{
    private TcpListener _serverSocket { get; set; }
    private List<TcpClient> _clients { get; set; } = new ();
    
    public void Start()
    {
        _serverSocket = new TcpListener(IPAddress.Loopback, 6969);
        _serverSocket.Start();
        
        Console.WriteLine("Server started!");
    }

    public async Task AcceptClientAsync()
    {
        TcpClient client = await _serverSocket.AcceptTcpClientAsync();
        _clients.Add(client);

        await SendMessageAsync(client, "Hello, I have accepted you.");
        
        Console.WriteLine("Server accepted a new client!");
    }

    public void Stop()
    {
        _serverSocket.Stop();
        
        Console.WriteLine("Server stopped!");
    }

    private async Task SendMessageAsync(TcpClient client, string message)
    {
        NetworkStream stream = client.GetStream();
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        await stream.WriteAsync(bytes, 0, bytes.Length);
        Console.WriteLine($"Sent message {message} to a client");
    }

    public async Task BroadcastMessageAsync(string message)
    {
        List<Task> tasks = new List<Task>();
        
        foreach (TcpClient client in _clients)
        {
            tasks.Add(SendMessageAsync(client, message));
        }

        await Task.WhenAll(tasks);
    }
}