using Microsoft.Extensions.DependencyInjection;
using QuizServer.Styles.UI_Wrappers;

namespace QuizServer.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.QuizRelevant.Entities.Questions;


public partial class QuestionEditorViewModel : ObservableObject
{
    private readonly ServerState _serverState;
    private readonly IQuizRepository _quizRepository;
    private readonly DashboardWindowViewModel _parentShell;

    [ObservableProperty] private QuestionListItem? _selectedItem;
    [ObservableProperty] private string _selectedTypeName = "Single Choice";
    [ObservableProperty] private string _newOptionText = string.Empty;

    public Quiz CurrentQuiz => _serverState.CurrentEditingQuiz ?? throw new InvalidOperationException("No quiz initialized in server state container workspace.");
    public ObservableCollection<QuestionListItem> QuestionsList { get; } = new();
    public ObservableCollection<string> QuestionTypes { get; } = new() { "Single Choice", "Multiple Choice", "True / False", "Short Text" };

    public bool IsQuestionSelected => SelectedItem != null;
    public bool ShowOptionsEditor => SelectedItem?.UnderlyingQuestion is SingleChoiceQuestion or MultiChoiceQuestion;
    public bool ShowTrueFalseEditor => SelectedItem?.UnderlyingQuestion is TrueFalseQuestion;
    public bool ShowShortTextEditor => SelectedItem?.UnderlyingQuestion is ShortTextQuestion;

    public QuestionEditorViewModel(ServerState serverState, IQuizRepository quizRepository, DashboardWindowViewModel parentShell)
    {
        _serverState = serverState;
        _quizRepository = quizRepository;
        _parentShell = parentShell;

        // Initialize view list matching current structural reference content array
        SynchronizeList();
        SelectedItem = QuestionsList.FirstOrDefault();
    }

    private void SynchronizeList()
    {
        QuestionsList.Clear();
        int counter = 1;
        foreach (var q in CurrentQuiz.Questions)
        {
            QuestionsList.Add(new QuestionListItem(q, counter++));
        }
    }

    [RelayCommand]
    private void AddQuestion()
    {
        var newQuestion = new SingleChoiceQuestion 
        { 
            Title = string.Empty, 
            TimeLimit = TimeSpan.FromSeconds(30),
            PointsWeight = 100 
        };
        
        CurrentQuiz.Questions.Add(newQuestion);
        SynchronizeList();
        
        SelectedItem = QuestionsList.LastOrDefault();
    }

    [RelayCommand]
    private void DeleteQuestion()
    {
        if (SelectedItem == null) return;
        
        CurrentQuiz.Questions.Remove(SelectedItem.UnderlyingQuestion);
        int oldIndex = QuestionsList.IndexOf(SelectedItem);
        
        SynchronizeList();
        
        int nextTarget = Math.Max(0, oldIndex - 1);
        SelectedItem = QuestionsList.Count > 0 ? QuestionsList[nextTarget] : null;
    }

    [RelayCommand]
    private void MoveUp()
    {
        if (SelectedItem == null) return;
        int idx = CurrentQuiz.Questions.IndexOf(SelectedItem.UnderlyingQuestion);
        if (idx <= 0) return;

        var temp = CurrentQuiz.Questions[idx];
        CurrentQuiz.Questions[idx] = CurrentQuiz.Questions[idx - 1];
        CurrentQuiz.Questions[idx - 1] = temp;

        SynchronizeList();
        SelectedItem = QuestionsList.FirstOrDefault(q => q.UnderlyingQuestion.Id == temp.Id);
    }

    [RelayCommand]
    private void MoveDown()
    {
        if (SelectedItem == null) return;
        int idx = CurrentQuiz.Questions.IndexOf(SelectedItem.UnderlyingQuestion);
        if (idx < 0 || idx >= CurrentQuiz.Questions.Count - 1) return;

        var temp = CurrentQuiz.Questions[idx];
        CurrentQuiz.Questions[idx] = CurrentQuiz.Questions[idx + 1];
        CurrentQuiz.Questions[idx + 1] = temp;

        SynchronizeList();
        SelectedItem = QuestionsList.FirstOrDefault(q => q.UnderlyingQuestion.Id == temp.Id);
    }

    partial void OnSelectedItemChanged(QuestionListItem? value)
    {
        OnPropertyChanged(nameof(IsQuestionSelected));
        OnPropertyChanged(nameof(ShowOptionsEditor));
        OnPropertyChanged(nameof(ShowTrueFalseEditor));
        OnPropertyChanged(nameof(ShowShortTextEditor));

        if (value == null) return;

        // Sync dropdown without triggering immediate cycle change mutations
        _selectedTypeName = value.UnderlyingQuestion switch
        {
            MultiChoiceQuestion => "Multiple Choice",
            TrueFalseQuestion => "True / False",
            ShortTextQuestion => "Short Text",
            _ => "Single Choice"
        };
        OnPropertyChanged(nameof(SelectedTypeName));
    }

    partial void OnSelectedTypeNameChanged(string value)
    {
        if (SelectedItem == null) return;
        var source = SelectedItem.UnderlyingQuestion;

        // Escape mutating identical classifications
        string currentType = source switch
        {
            MultiChoiceQuestion => "Multiple Choice",
            TrueFalseQuestion => "True / False",
            ShortTextQuestion => "Short Text",
            _ => "Single Choice"
        };
        if (currentType == value) return;

        Question target;
        switch (value)
        {
            case "Multiple Choice":
                target = new MultiChoiceQuestion { Options = source.Options.ToList() };
                break;
            case "True / False":
                target = new TrueFalseQuestion 
                { 
                    Options = new() { new QuestionOption(true, "True"), new QuestionOption(false, "False") } 
                };
                break;
            case "Short Text":
                target = new ShortTextQuestion { CorrectText = string.Empty, CaseSensitive = false };
                break;
            default:
                target = new SingleChoiceQuestion { Options = source.Options.ToList() };
                break;
        }

        // Map core values across types
        target.Id = source.Id;
        target.Title = source.Title;
        target.PointsWeight = source.PointsWeight;
        target.TimeLimit = source.TimeLimit;

        int quizIndex = CurrentQuiz.Questions.IndexOf(source);
        CurrentQuiz.Questions[quizIndex] = target;

        int listIndex = QuestionsList.IndexOf(SelectedItem);
        var updatedWrapper = new QuestionListItem(target, listIndex + 1);
        QuestionsList[listIndex] = updatedWrapper;
        SelectedItem = updatedWrapper;
    }

    public void UpdateActiveTitle(string title)
    {
        if (SelectedItem == null) return;
        SelectedItem.UnderlyingQuestion.Title = title;
        SelectedItem.UpdateTitle();
    }

    public void UpdateActiveTimeLimit(string secondsStr)
    {
        if (SelectedItem == null) return;
        if (int.TryParse(secondsStr, out int sec))
        {
            SelectedItem.UnderlyingQuestion.TimeLimit = TimeSpan.FromSeconds(sec);
        }
    }

    [RelayCommand]
    private void AddOption()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(NewOptionText)) return;
        SelectedItem.UnderlyingQuestion.Options.Add(new QuestionOption(false, NewOptionText));
        NewOptionText = string.Empty;
        TriggerOptionsUIRefresh();
    }

    [RelayCommand]
    private void RemoveOption(QuestionOption option)
    {
        if (SelectedItem == null) return;
        SelectedItem.UnderlyingQuestion.Options.Remove(option);
        TriggerOptionsUIRefresh();
    }

    public void ToggleSingleChoiceOptionSelection(QuestionOption rawOption)
    {
        if (SelectedItem?.UnderlyingQuestion is not SingleChoiceQuestion) return;
        
        foreach (var op in SelectedItem.UnderlyingQuestion.Options)
        {
            // Set input option and reset all other options
            var trackingObjectHack = op;
            typeof(QuestionOption).GetProperty(nameof(QuestionOption.IsCorrect))?
                .SetValue(trackingObjectHack, op.Id == rawOption.Id);
        }
        TriggerOptionsUIRefresh();
    }

    public void ToggleMultiChoiceOptionSelection(QuestionOption rawOption)
    {
        if (SelectedItem?.UnderlyingQuestion is not MultiChoiceQuestion) return;
        
        var trackingObjectHack = rawOption;
        typeof(QuestionOption).GetProperty(nameof(QuestionOption.IsCorrect))?
            .SetValue(trackingObjectHack, !rawOption.IsCorrect);
        
        TriggerOptionsUIRefresh();
    }

    public void ToggleTrueFalseOptionSelection(bool chooseTrueCorrect)
    {
        if (SelectedItem?.UnderlyingQuestion is not TrueFalseQuestion tf) return;
        if (tf.Options.Count != 2) return;

        var trueOp = tf.Options.FirstOrDefault(o => o.TextContent == "True");
        var falseOp = tf.Options.FirstOrDefault(o => o.TextContent == "False");

        if (trueOp != null && falseOp != null)
        {
            typeof(QuestionOption).GetProperty(nameof(QuestionOption.IsCorrect))?.SetValue(trueOp, chooseTrueCorrect);
            typeof(QuestionOption).GetProperty(nameof(QuestionOption.IsCorrect))?.SetValue(falseOp, !chooseTrueCorrect);
        }
        TriggerOptionsUIRefresh();
    }

    private void TriggerOptionsUIRefresh()
    {
        var current = SelectedItem;
        SelectedItem = null;
        SelectedItem = current;
    }

    [RelayCommand]
    private async Task SaveQuiz()
    {
        if (CurrentQuiz.Questions.Count == 0) return;
        
        var existingQuiz = await _quizRepository.GetQuizByGuidAsync(CurrentQuiz.Id);
        
        Console.WriteLine(CurrentQuiz.Questions.Count);
        foreach(var q in CurrentQuiz.Questions)
        {
            Console.WriteLine(q.Title);
        }

        if (existingQuiz != null)
        {
            await _quizRepository.UpdateAsync(CurrentQuiz);
        }
        else
        {
            await _quizRepository.CreateAsync(CurrentQuiz);
        }
        _serverState.CurrentEditingQuiz = null;
        _parentShell.NavigateToQuizzes();
    }

    [RelayCommand]
    private void Cancel()
    {
        _serverState.CurrentEditingQuiz = null;
        _parentShell.NavigateToQuizzes();
    }
}