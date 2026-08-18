using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.Services.Interfaces;

namespace QuizServer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class DashboardWindowViewModel : ObservableObject
{
    // The active user details displayed in the constant top header
    [ObservableProperty] private string _currentTeacherName;

    // This property drives the structural ContentControl in the middle of the frame
    [ObservableProperty] private ObservableObject? _currentWorkspaceViewModel;

    public DashboardWindowViewModel()
    {
        NavigateToDashboard();
    }

    [RelayCommand]
    public void NavigateToDashboard()
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        _currentTeacherName = serverState.Account.UserName;
        CurrentWorkspaceViewModel = new MainDashboardViewModel(this);
    }

    [RelayCommand]
    public void NavigateToQuizzes()
    {
        // Placeholder for the Tests module page context
        CurrentWorkspaceViewModel = Program.AppHost.Services.GetRequiredService<QuizBrowserViewModel>();
    }

    [RelayCommand]
    public void NavigateToConstructor()
    {
        // Placeholder for your in-memory constructor editor view
        CurrentWorkspaceViewModel = Program.AppHost.Services.GetRequiredService<QuizBrowserViewModel>();
    }
    
    [RelayCommand]
    public void NavigateToDetailsView()
    {
        // Placeholder for your in-memory constructor editor view
        CurrentWorkspaceViewModel = new QuizDetailsViewModel(this);
    }

    [RelayCommand]
    public void NavigateToSessions()
    {
        // CurrentWorkspaceViewModel = new SessionsViewModel();
    }

    [RelayCommand]
    public void NavigateToParticipants()
    {
        // CurrentWorkspaceViewModel = new ParticipantsViewModel();
    }

    [RelayCommand]
    public void NavigateToReports()
    {
        // CurrentWorkspaceViewModel = new ReportsViewModel();
    }

    [RelayCommand]
    public void NavigateToSettings()
    {
        // CurrentWorkspaceViewModel = new SettingsViewModel();
    }

    [RelayCommand]
    private void ExecuteLogout()
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        serverState.CurrentEditingQuiz = null;
        serverState.Account = null;
        INavigationService navigationService = Program.AppHost.Services.GetRequiredService<INavigationService>();
        navigationService.NavigateTo<LoginViewModel>();
    }
}