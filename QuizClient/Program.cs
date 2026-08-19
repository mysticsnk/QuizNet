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
using QuizClient.Models.ConnectionRelevant.Entities.ServerMessages;
using QuizClient.Models.DatabaseRelevant.Entities;
using QuizClient.Models.DatabaseRelevant.Interfaces;
using QuizClient.Models.Entities;
using QuizClient.Models.Entities.QuizRelevant;
using QuizClient.Models.QuizRelevant.Abstracts;
using QuizClient.Models.QuizRelevant.Entities.Questions;
using QuizClient.Models.SessionRelevant;
using QuizClient.Models.SessionRelevant.Answers;
using QuizClient.Models.UserRelevant;
using QuizClient.Services;
using QuizClient.Services.Interfaces;
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

        Console.WriteLine(typeof(AvaloniaObject).Assembly.GetName().Version);
        
        

        IPasswordHashingService hasher = AppHost.Services.GetRequiredService<IPasswordHashingService>();
        SocketClient client = AppHost.Services.GetRequiredService<SocketClient>();
        await client.ConnectToServerAsync();
        _ = client.StartAcceptLoopAsync();
        
        string userName = "gudli";
        string email = "mystic@example.com";
        string password = "superSecret123";

        string hash = hasher.Hash(password);
        await Task.Delay(5000);

        UserAccount account = await client.RegisterAsync(userName, email, hash);
        
        ClientState clientState = AppHost.Services.GetRequiredService<ClientState>();
        
        Console.WriteLine(clientState.Account);
        
        Task<Question> questionTask = client.WaitForNextQuestionAsync();

        await client.JoinQuizAsync(userName, "1234", account);

        Question currentQuestion = await questionTask;
        
        int questionCounter = 1;
        while (questionCounter <= clientState.CurrentSession.Quiz.Questions.Count)
        {
            Console.WriteLine(clientState.CurrentSession.CurrentQuestion?.Title ?? "No question yet");
            
            Task<Question> nextQuestionTask = client.WaitForNextQuestionAsync();
            
            if (currentQuestion is SingleChoiceQuestion singleChoiceQuestion)
            {
                SingleChoiceAnswer answer =
                    new SingleChoiceAnswer(currentQuestion.Id, singleChoiceQuestion.Options[0].Id);
            
                await client.SendAnswerAsync(answer);
                Console.WriteLine("Sent a single choice answer!");
            }
            else if (currentQuestion is MultiChoiceQuestion multiChoiceQuestion)
            {
                List<Guid> options = new List<Guid>();
                options.Add(multiChoiceQuestion.Options[0].Id);
                options.Add(multiChoiceQuestion.Options[1].Id);
                MultiChoiceAnswer answer = new MultiChoiceAnswer(currentQuestion.Id, options);
                
                await client.SendAnswerAsync(answer);
                Console.WriteLine("Sent a multi choice answer!");
            }
            else if (currentQuestion is ShortTextQuestion shortTextQuestion)
            {
                string answerText = "Albert Einstein";
                ShortTextAnswer answer = new ShortTextAnswer(currentQuestion.Id, answerText);

                await client.SendAnswerAsync(answer);
                Console.WriteLine("Sent a short text answer!");
            }
            else if (currentQuestion is TrueFalseQuestion trueFalseQuestion)
            {
                QuestionOption option = trueFalseQuestion.Options[0];
                TrueFalseAnswer answer = new TrueFalseAnswer(currentQuestion.Id, option.Id);

                await client.SendAnswerAsync(answer);
                Console.WriteLine("Sent a true false answer!");
            }

            questionCounter++;

            currentQuestion = await nextQuestionTask;
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
        // TODO: Make an actual anticheat
        services.AddTransient<IAnticheatApplyService, DummyAnticheatService>();
        
        // TODO: Make an actual port resolver
        services.AddSingleton<IPortResolver, DummyPortResolver>();
        services.AddSingleton<SocketClient>();
        services.AddSingleton<ClientState>();
        
        string connectionString =
            $"Data Source={Path.Combine(AppContext.BaseDirectory, "quiz.db")}";
        services.AddDbContext<QuizDbContext>(options => options.UseSqlite(connectionString));
        
    }
}