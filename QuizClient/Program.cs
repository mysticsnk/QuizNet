using Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizClient.Models;
using QuizClient.Models.DatabaseRelevant.Entities;
using QuizClient.Models.DatabaseRelevant.Interfaces;
using QuizClient.Models.Entities;
using QuizClient.Models.Helpers.Password;
using QuizClient.Models.Interfaces;
using QuizClient.Models.Services;
using QuizClient.Models.Services.Interfaces;
using QuizClient.Models.UserRelevant;
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
        
        UserValidationService userValidationService = AppHost.Services.GetRequiredService<UserValidationService>();
        IUserRepository userRepository = AppHost.Services.GetRequiredService<IUserRepository>();
        IQuizRepository quizRepository = AppHost.Services.GetRequiredService<IQuizRepository>();
        IUserRegistrationService registrationService = AppHost.Services.GetRequiredService<IUserRegistrationService>();
        
        Console.WriteLine("Registering user...");

        await registrationService.RegisterAsync(
            "Bogdan",
            "bogdan@test.com",
            "SuperSecret123");

        Console.WriteLine("User registered!");

        Console.WriteLine();

        Console.WriteLine("All users in database:");

        List<UserAccount> users = await userRepository.GetAllUsersAsync();

        foreach (UserAccount user in users)
        {
            Console.WriteLine($"Username: {user.UserName}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Password hash: {user.PasswordHash}");
            Console.WriteLine();
        }

        Console.WriteLine("Trying to verify password...");

        try
        {
            await registrationService.VerifyAsync(
                "Bogdan",
                "bogdan@test.com",
                "SuperSecret123");

            Console.WriteLine("Password verified!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        
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
        
        services.AddSingleton<IPortResolver, DummyPortResolver>();
        
        string connectionString =
            $"Data Source={Path.Combine(AppContext.BaseDirectory, "quiz.db")}";
        services.AddDbContext<QuizDbContext>(options => options.UseSqlite(connectionString));
        
    }
}