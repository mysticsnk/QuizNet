using Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizServer.Models.DatabaseRelevant.Entities;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Entities;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.Helpers.Password;
using QuizServer.Models.Interfaces;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.QuizRelevant.Entities.Questions;
using QuizServer.Models.QuizRelevant.Entities.QuizModes;
using QuizServer.Models.Services;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.SessionRelevant;
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
        
        
        UserAccount account = new UserAccount(
            "MysticSNKhost",
            "mystic@example.com",
            "dummyHash"
        );
        
        var quiz = new Quiz(
            "General Knowledge Test",
            new List<Question>
            {
                new SingleChoiceQuestion(
                    new List<QuestionOption>
                    {
                        new(true, "Paris"),
                        new(false, "London"),
                        new(false, "Berlin"),
                        new(false, "Madrid")
                    },
                    "What is the capital of France?",
                    pointsWeight: 100
                ),

                new MultiChoiceQuestion(
                    new List<QuestionOption>
                    {
                        new(true, "C#"),
                        new(true, "Java"),
                        new(false, "HTML"),
                        new(false, "CSS")
                    },
                    "Which of the following are programming languages?",
                    pointsWeight: 150
                ),

                new TrueFalseQuestion(
                    new List<QuestionOption>
                    {
                        new(false, "True"),
                        new(true, "False")
                    },
                    "The Sun revolves around the Earth.",
                    pointsWeight: 50
                ),

                new ShortTextQuestion(
                    "Who developed the theory of relativity?",
                    null,
                    200,
                    "Albert Einstein",
                    false,
                    "Type the scientist's name..."
                )
            }
        );

        SocketServer server = AppHost.Services.GetRequiredService<SocketServer>();
        IQuizRepository quizRepository = AppHost.Services.GetRequiredService<IQuizRepository>();
        await quizRepository.CreateAsync(quiz);
        
        Task serverTask = server.StartAsync();

        SynchronousQuizMode mode = new SynchronousQuizMode();
        ServerQuizSession session = new ServerQuizSession(quiz, "1234", mode);

        await session.StartAsync();

        await Task.Delay(10000);

        await mode.AdvanceQuestionAsync();
        Console.WriteLine("Advanced to the next question");
        
        await Task.Delay(3000);

        await mode.AdvanceQuestionAsync();
        Console.WriteLine("Advanced to the next question");
        
        await Task.Delay(3000);

        await mode.AdvanceQuestionAsync();
        Console.WriteLine("Advanced to the next question");
        
        await Task.Delay(3000);

        await mode.AdvanceQuestionAsync();
        Console.WriteLine("Advanced to the next question");
        
        await Task.Delay(3000);

        await mode.AdvanceQuestionAsync(); 
        Console.WriteLine("Advanced to the next question");
        
        
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
        services.AddTransient<IHandleClientRegistrationService, HandleClientRegistrationService>();
        services.AddTransient<IHandleClientLoginService, HandleClientLoginService>();
        services.AddTransient<IHandleClientQuizJoinService, HandleClientQuizJoinService>();
        services.AddTransient<IAnswerLogger, DummyAnswerLogger>();
        services.AddTransient<ICheckAnswerService, CheckAnswerService>();
        
        services.AddSingleton<IPortResolver, DummyPortResolver>();
        services.AddSingleton<SocketServer>();
        services.AddSingleton<ServerState>();
        
        string connectionString =
            $"Data Source={Path.Combine(AppContext.BaseDirectory, "quiz.db")}";
        services.AddDbContext<QuizDbContext>(options => options.UseSqlite(connectionString)
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ContextInitialized)));
        
    }
}