namespace QuizServer.Styles.UI_Wrappers;

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using QuizServer.Models.QuizRelevant.Abstracts;

public partial class QuestionListItem : ObservableObject
{
    public Question UnderlyingQuestion { get; }
    
    [ObservableProperty] private int _number;
    [ObservableProperty] private string _displayTitle = string.Empty;

    public QuestionListItem(Question question, int number)
    {
        UnderlyingQuestion = question;
        _number = number;
        UpdateTitle();
    }

    public void UpdateTitle()
    {
        DisplayTitle = string.IsNullOrWhiteSpace(UnderlyingQuestion.Title) 
            ? "Untitled Question" 
            : UnderlyingQuestion.Title;
    }
}