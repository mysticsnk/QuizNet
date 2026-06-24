using Avalonia;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using QuizClient.Models;

namespace QuizClient;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
        SocketClient client = new SocketClient();
        string message = await client.ReadMessageAsync();
        Console.WriteLine(message);
        string secondMessage = await client.ReadMessageAsync();
        Console.WriteLine(secondMessage);
        
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}