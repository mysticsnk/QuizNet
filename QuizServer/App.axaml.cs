using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using QuizServer.ViewModels;
using QuizServer.Views;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.Entities;
using QuizServer.Models.Interfaces;

namespace QuizServer;

public partial class  App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }
    
    public override void Initialize()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder.Services.AddSingleton<IPortResolver, SpecificPortResolver>();
        IHost host = builder.Build();
        host.RunAsync();
        ServiceProvider = host.Services;
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(ServiceProvider.GetRequiredService<IPortResolver>()),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}