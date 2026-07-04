using Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizServer.Models.DatabaseRelevant.Entities;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Entities;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.Helpers.Password;
using QuizServer.Models.Interfaces;
using QuizServer.Models.QuizRelevant.Entities.Questions;
using QuizServer.Models.Services;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.UserRelevant;
using QuizServer.ViewModels;
using Host = QuizServer.Models.SessionRelevant.Host;

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
        AppHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services);
            })
            .Build();
        
        
        UserAccount account = new UserAccount(
            "MysticSNKhost",
            "mystic@example.com",
            "dummyHash"
        );

        Host host = new Host(account);
        await host.BeginAcceptingClients();
        
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
        services.AddTransient<IUserRegistrationService, UserRegistrationService>();
        services.AddTransient<IUserValidationService, UserValidationService>();
        services.AddTransient<IHandleClientRegistrationService, DummyHandleClientRegistrationService>();
        services.AddTransient<IHandleClientLoginService, DummyHandleClientLoginService>();
        
        services.AddSingleton<IPortResolver, DummyPortResolver>();
        services.AddSingleton<SocketServer>();
        
        string connectionString =
            $"Data Source={Path.Combine(AppContext.BaseDirectory, "quiz.db")}";
        services.AddDbContext<QuizDbContext>(options => options.UseSqlite(connectionString));
        
    }
}