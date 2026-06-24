using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizServer.Models.Entities;
using QuizServer.Models.Interfaces;

namespace QuizServer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase 
{
    [ObservableProperty]
    public partial string Port { get; set; }

    public MainWindowViewModel(IPortResolver portResolver)
    {
        Port = portResolver.GetPort();
    }
}

