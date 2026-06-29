using Avalonia;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizClient.Models;
using QuizClient.Models.Entities;
using QuizClient.Models.Interfaces;
using QuizClient.ViewModels;

namespace QuizClient;

sealed class Program
{
    public static IHost AppHost { get; private set; }
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
        AppHost = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services);
            })
            .Build();

        SocketClient client = new SocketClient("Bogdan", AppHost.Services.GetRequiredService<IPortResolver>());
        Console.WriteLine(client._portResolver.GetPort());
        
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

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SocketClient>();

        services.AddSingleton<IPortResolver, DummyPortResolver>();
    }
}