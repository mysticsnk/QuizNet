using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizServer.Models.Entities;
using QuizServer.Models.Interfaces;
using QuizServer.Models.Services.Interfaces;

namespace QuizServer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private INavigationService _navigation;

    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigation = navigationService;
        
        // Set your initial landing view on startup
        _navigation.NavigateTo<LoginViewModel>();
    }
}
