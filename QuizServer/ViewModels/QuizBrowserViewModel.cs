using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.QuizRelevant.Entities;

namespace QuizServer.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizServer.Models.QuizRelevant;
using QuizServer.Models.DatabaseRelevant.Interfaces;


public partial class QuizBrowserViewModel : ObservableObject
{
    private readonly IQuizRepository _quizRepository;
    private readonly DashboardWindowViewModel _parentShell;

    [ObservableProperty] private QuizListItem? _selectedQuiz;
    [ObservableProperty] private bool _isLoading;

    public ObservableCollection<QuizListItem> Quizzes { get; } = new();

    // Derived properties to clean up view visibility checks
    public bool IsQuizSelected => SelectedQuiz != null;

    public QuizBrowserViewModel(IQuizRepository quizRepository, DashboardWindowViewModel parentShell)
    {
        _quizRepository = quizRepository;
        _parentShell = parentShell;

        // Populate database records asynchronously on load
        _ = RefreshQuizzesAsync();
    }

    [RelayCommand]
    public async Task RefreshQuizzesAsync()
    {
        IsLoading = true;
        Quizzes.Clear();

        try
        {
            // Fetch all database entries
            var rawQuizzes = await _quizRepository.GetAllQuizzesAsync();
            
            foreach (var q in rawQuizzes)
            {
                // QuestionCount extraction maps naturally here. 
                // Note: Ensure your repo or context includes the Questions collection
                int count = q.Questions?.Count ?? 0; 
                Quizzes.Add(new QuizListItem(q.Id, q.Title, count));
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void CreateQuiz()
    {
        // Route frame engine over to your Quiz Details configuration view
        _parentShell.CurrentWorkspaceViewModel = new QuizDetailsViewModel(_parentShell);
    }

    [RelayCommand(CanExecute = nameof(IsQuizSelected))]
    private async Task EditQuiz()
    {
        if (SelectedQuiz == null) return;

        // Fetch complete deep entity from storage repository matching unique Guid identity
        var completeQuiz = await _quizRepository.GetQuizByGuidAsync(SelectedQuiz.Id);
        if (completeQuiz == null) return;
        
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        serverState.CurrentEditingQuiz = completeQuiz;

        // Route complete configuration down directly into the deep multi-mode question constructor view
        _parentShell.CurrentWorkspaceViewModel = Program.AppHost.Services.GetRequiredService<QuestionEditorViewModel>();
    }

    [RelayCommand(CanExecute = nameof(IsQuizSelected))]
    private async Task DeleteQuiz()
    {
        if (SelectedQuiz == null) return;

        // Execute persistence deletion operation routine 
        bool success = await _quizRepository.DeleteAsync(SelectedQuiz.Id);
        
        if (success)
        {
            SelectedQuiz = null;
            await RefreshQuizzesAsync();
        }
    }

    partial void OnSelectedQuizChanged(QuizListItem? value)
    {
        // Re-evaluate authorization execution parameters for buttons
        EditQuizCommand.NotifyCanExecuteChanged();
        DeleteQuizCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(IsQuizSelected));
    }
}