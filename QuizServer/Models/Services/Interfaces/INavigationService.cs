using CommunityToolkit.Mvvm.ComponentModel;

namespace QuizServer.Models.Services.Interfaces;

public interface INavigationService
{
    // The current active screen driving the UI window frame
    ObservableObject? CurrentViewModel { get; }
    
    // Call this to switch screens seamlessly
    void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
}