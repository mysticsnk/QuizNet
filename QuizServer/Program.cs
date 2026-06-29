using Avalonia;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizServer.Models.DatabaseRelevant.Entities;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Entities;
using QuizServer.Models.Helpers.Password;
using QuizServer.Models.Interfaces;
using QuizServer.Models.Services;
using QuizServer.Models.UserRelevant;
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

        UserValidationService userValidationService = AppHost.Services.GetRequiredService<UserValidationService>();
        IUserRepository userRepository = AppHost.Services.GetRequiredService<IUserRepository>();

        Console.WriteLine("Enter username:");
        string userName = "Bogdan";
        
        Console.WriteLine("Enter email:");
        string email = "mi@gmail.com";
        if (!userValidationService.IsValidEmail(email))
        {
            Console.WriteLine("Invalid email");
            Environment.Exit(1);
        }
        
        Console.WriteLine("Enter password:");
        string password = "SuperMario";
        if (!userValidationService.IsValidPassword(password))
        {
            Console.WriteLine("Invalid password");
            Environment.Exit(1);
        }

        UserAccount user = new UserAccount(userName, email, password);

        Guid id = user.Id;

        await userRepository.CreateAsync(user);
        Console.WriteLine("Created user successfully!");

        UserAccount? foundUser = await userRepository.GetUserByGuidAsync(id);

        if (foundUser == null)
        {
            Console.WriteLine("User wasn't found!");
            Environment.Exit(1);
        }

        Console.WriteLine(foundUser);
        
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
        services.AddTransient<PasswordValidator>();
        services.AddTransient<UserValidationService>();
        services.AddTransient<IUserRepository, SqliteUserRepository>();
        
        services.AddSingleton<IPortResolver, DummyPortResolver>();
        services.AddSingleton<SocketServer>();
        
        string connectionString =
            $"Data Source={Path.Combine(AppContext.BaseDirectory, "quiz.db")}";
        services.AddDbContext<QuizDbContext>(options => options.UseSqlite(connectionString));
        
    }
}