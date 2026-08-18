using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Entities.Questions;
using QuizServer.ViewModels;

namespace QuizServer.Views;

public partial class QuestionEditorView : UserControl
{
    public QuestionEditorView()
    {
        InitializeComponent();
    }

    private void OnTitleTextInputChanged(object? sender, TextInputEventArgs e)
    {
        if (sender is TextBox box && DataContext is QuestionEditorViewModel vm)
        {
            // Push text down through input interface proxy hooks manually to refresh the left pane title string
            vm.UpdateActiveTitle(box.Text ?? string.Empty);
        }
    }

    private void OnTimeLimitInputChanged(object? sender, TextInputEventArgs e)
    {
        if (sender is TextBox box && DataContext is QuestionEditorViewModel vm)
        {
            vm.UpdateActiveTimeLimit(box.Text ?? "30");
        }
    }

    private void OnOptionAnswerStateToggleClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is QuestionOption op && DataContext is QuestionEditorViewModel vm)
        {
            if (vm.SelectedItem?.UnderlyingQuestion is SingleChoiceQuestion)
            {
                vm.ToggleSingleChoiceOptionSelection(op);
            }
            else if (vm.SelectedItem?.UnderlyingQuestion is MultiChoiceQuestion)
            {
                vm.ToggleMultiChoiceOptionSelection(op);
            }
        }
    }

    private void OnTrueRadioButtonChecked(object? sender, RoutedEventArgs e)
    {
        // Make sure the sender is actually checked, not unchecked!
        if (sender is RadioButton rb && rb.IsChecked == true && DataContext is QuestionEditorViewModel vm)
        {
            vm.ToggleTrueFalseOptionSelection(chooseTrueCorrect: true);
        }
    }

    private void OnFalseRadioButtonChecked(object? sender, RoutedEventArgs e)
    {
        if (sender is RadioButton rb && rb.IsChecked == true && DataContext is QuestionEditorViewModel vm)
        {
            vm.ToggleTrueFalseOptionSelection(chooseTrueCorrect: false);
        }
    }
}