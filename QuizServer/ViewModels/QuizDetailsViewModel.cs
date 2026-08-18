using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.Entities.QuizRelevant;

namespace QuizServer.ViewModels;

public partial class QuizDetailsViewModel : ObservableObject
{
    private readonly DashboardWindowViewModel _parentShell;

    [ObservableProperty] private string _quizTitle = string.Empty;

    // The Continue button evaluates this property to toggle its execution/enabled state
    public bool IsTitleValid => !string.IsNullOrWhiteSpace(QuizTitle);

    public QuizDetailsViewModel(DashboardWindowViewModel parentShell)
    {
        _parentShell = parentShell;
        Console.WriteLine(_parentShell.CurrentWorkspaceViewModel?.GetType().Name);
    }

    [RelayCommand(CanExecute = nameof(IsTitleValid))]
    private void ContinueToConstructor()
    {
        if (!IsTitleValid) return;

        // 1. Initialize a completely clean Quiz object in volatile memory
        var newQuiz = new Quiz
        {
            Title = QuizTitle.Trim()
        };

        // 2. Assign it to ServerState to serve as the unified pipeline source
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        serverState.CurrentEditingQuiz = newQuiz;

        // 3. Shift the parent workspace shell view forward onto the construction panel
        _parentShell.CurrentWorkspaceViewModel = Program.AppHost.Services.GetRequiredService<QuestionEditorViewModel>();
    }

    [RelayCommand]
    private void CancelAndReturn()
    {
        // Discard allocations and route backwards straight into the collection list browser view
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        serverState.CurrentEditingQuiz = null;
        
        _parentShell.NavigateToQuizzes();
    }

    partial void OnQuizTitleChanged(string value)
    {
        // Alert the primary continue trigger that validation rules need re-evaluating
        ContinueToConstructorCommand.NotifyCanExecuteChanged();
    }
}