using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace QuizServer.ViewModels;

public partial class MainDashboardViewModel : ObservableObject
{
    private readonly DashboardWindowViewModel _parentShell;

    public MainDashboardViewModel(DashboardWindowViewModel parentShell)
    {
        _parentShell = parentShell;
    }

    [RelayCommand] private void ForwardToTests() => _parentShell.NavigateToQuizzes();
    [RelayCommand] private void ForwardToConstructor() => _parentShell.NavigateToConstructor();
    [RelayCommand] private void ForwardToSessions() => _parentShell.NavigateToSessions();
    [RelayCommand] private void ForwardToParticipants() => _parentShell.NavigateToParticipants();
    [RelayCommand] private void ForwardToReports() => _parentShell.NavigateToReports();
    [RelayCommand] private void ForwardToSettings() => _parentShell.NavigateToSettings();
}