using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuizServer.Models.Entities;

public class SocketServer
{
    private HttpListener _serverSocket { get; set; }
    private List<TcpClient> _clients { get; set; } = new ();
    
    public void Start()
    {
        _serverSocket = new HttpListener();
        _serverSocket.Prefixes.Add("http://localhost:6969/");
        _serverSocket.Start();
        
        Console.WriteLine("Server started!");
    }

    // public async Task AcceptClientAsync()
    // {
    //     TcpClient client = await _serverSocket.();
    //     _clients.Add(client);
    //     
    //     await SendMessageAsync(client, "Hello, I have accepted you.");
    //     
    //     Console.WriteLine("Server accepted a new client!");
    // }

    public void Stop()
    {
        _serverSocket.Stop();
        
        Console.WriteLine("Server stopped!");
    }

    public async Task HandleMessageAsync()
    {
        HttpListenerContext context = await _serverSocket.GetContextAsync();
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        
        Stream inputStream = request.InputStream;
        StreamReader reader = new StreamReader(inputStream);
        string message = await reader.ReadToEndAsync();
        string responseMessage = $"Echo: {message}";
        byte[] bytes = Encoding.ASCII.GetBytes(responseMessage);

        response.StatusCode = 200;
        response.ContentType = "text/plain";
        response.ContentLength64 = bytes.Length;
        
        await response.OutputStream.WriteAsync(bytes);
        response.Close();
    }

    // public async Task BroadcastMessageAsync(string message)
    // {
    //     List<Task> tasks = new List<Task>();
    //     
    //     foreach (TcpClient client in _clients)
    //     {
    //         tasks.Add(SendMessageAsync(client, message));
    //     }
    //
    //     await Task.WhenAll(tasks);
    // }
}