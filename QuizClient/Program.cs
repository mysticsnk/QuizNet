using Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizClient.Models;
using QuizClient.Models.AppRelevant;
using QuizClient.Models.ConnectionRelevant.Entities.ClientMessages;
using QuizClient.Models.DatabaseRelevant.Entities;
using QuizClient.Models.DatabaseRelevant.Interfaces;
using QuizClient.Models.Entities;
using QuizClient.Models.Entities.QuizRelevant;
using QuizClient.Models.Helpers.Password;
using QuizClient.Models.Interfaces;
using QuizClient.Models.QuizRelevant.Abstracts;
using QuizClient.Models.QuizRelevant.Entities.Questions;
using QuizClient.Models.Services;
using QuizClient.Models.Services.Interfaces;
using QuizClient.ViewModels;

namespace QuizClient;

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
        
        

        IPasswordHashingService hasher = AppHost.Services.GetRequiredService<IPasswordHashingService>();
        SocketClient client = AppHost.Services.GetRequiredService<SocketClient>();
        await client.ConnectToServerAsync();
        Task.Run(async () =>
        {
            await client.StartAcceptLoopAsync();
        });
        
        string userName = "mystic@example.com";
        string email = "mystic@example.com";
        string password = "superSecret123";

        string hash = hasher.Hash(password);

        ClientRegistrationMessage registrationMessage = new ClientRegistrationMessage(userName, email, hash);
        await client.SendMessageAsync(registrationMessage);
        Thread.Sleep(500);
        
        ClientState clientState = AppHost.Services.GetRequiredService<ClientState>();

        int counter = 100;
        while (clientState.Account == null)
        {
            await Task.Delay(100);
            Console.WriteLine(counter);
            counter += 100;
        }
        
        Console.WriteLine(clientState.Account);

        ClientJoinQuizMessage joinMessage = new ClientJoinQuizMessage("Gudli", "1234", clientState.Account);
        await client.SendMessageAsync(joinMessage);
        
        
        counter = 100;
        while (clientState.CurrentSession == null)
        {
            await Task.Delay(100);
            Console.WriteLine(counter);
            counter += 100;
        }
        
        Console.WriteLine(clientState.CurrentSession.Participant.UserName);
        
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
        services.AddTransient<PasswordValidationService>();
        services.AddTransient<UserValidationService>();
        services.AddTransient<IUserRepository, SqliteUserRepository>();
        services.AddTransient<IQuizRepository, SqliteQuizRepository>();
        services.AddTransient<IPasswordHashingService, Sha256PasswordHashingService>();
        
        services.AddSingleton<IPortResolver, DummyPortResolver>();
        services.AddSingleton<SocketClient>();
        services.AddSingleton<ClientState>();
        
        string connectionString =
            $"Data Source={Path.Combine(AppContext.BaseDirectory, "quiz.db")}";
        services.AddDbContext<QuizDbContext>(options => options.UseSqlite(connectionString));
        
    }
}