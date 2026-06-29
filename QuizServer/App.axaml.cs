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
    public override void Initialize()
    {
        
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Program.AppHost.Services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}