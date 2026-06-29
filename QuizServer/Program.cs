using Avalonia;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizServer.Models.Entities;
using QuizServer.Models.Interfaces;
using QuizServer.ViewModels;

namespace QuizServer;

sealed class Program
{
    public static IHost? AppHost { get; private set; }
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

        SocketServer server = AppHost.Services.GetRequiredService<SocketServer>();
        
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
        services.AddTransient<SocketServer>();

        services.AddSingleton<IPortResolver, DummyPortResolver>();
    }
}