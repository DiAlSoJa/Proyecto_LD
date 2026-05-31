using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiAppLogin.Models;

public partial class ChecklistOption : ObservableObject
{
    public ChecklistOption(ChecklistQuestion question, string text)
    {
        Question = question;
        Text = text;
    }

    public ChecklistQuestion Question { get; }
    public string Text { get; }

    [ObservableProperty]
    private bool isSelected;
}
