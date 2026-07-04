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
using QuizClient.Models.Entities.QuizRelevant;
using QuizClient.Models.Helpers.Password;
using QuizClient.Models.Interfaces;
using QuizClient.Models.QuizRelevant.Abstracts;
using QuizClient.Models.QuizRelevant.Entities.Questions;
using QuizClient.Models.Services;
using QuizClient.Models.Services.Interfaces;
using QuizClient.Models.SessionRelevant;
using QuizClient.Models.SessionRelevant.Answers;
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
        
        UserAccount account = new UserAccount(
            "MysticSNK",
            "mystic@example.com",
            "dummyHash"
        );

        Participant participant = new Participant(
            account.UserName,
            account
        );

        if (quiz.Questions[0] is SingleChoiceQuestion singleChoiceQuestion)
        {
            SingleChoiceAnswer singleChoiceAnswer = new SingleChoiceAnswer(participant.Id, quiz.Questions[0].Id,
                singleChoiceQuestion.Options[0].Id);
            await participant.SendAnswerAsync(singleChoiceAnswer);
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