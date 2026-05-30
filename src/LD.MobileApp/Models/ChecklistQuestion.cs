using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiAppLogin.Models;

public partial class ChecklistQuestion : ObservableObject
{
    public int QuestionId { get; set; }
    public string Label { get; set; } = string.Empty;

    public ObservableCollection<string> Options { get; set; } = new();
    public ObservableCollection<ChecklistOption> OptionItems { get; set; } = new();

    [ObservableProperty]
    private string? selectedOption;

    public void AddOption(string text)
    {
        Options.Add(text);
        OptionItems.Add(new ChecklistOption(this, text));
    }

    public void SelectSingleOption(string text)
    {
        foreach (var option in OptionItems)
            option.IsSelected = option.Text == text;

        SelectedOption = text;
    }

    public void ToggleMultiOption(ChecklistOption selected)
    {
        selected.IsSelected = !selected.IsSelected;
        SelectedOption = string.Join(", ",
            OptionItems
                .Where(option => option.IsSelected)
                .Select(option => option.Text));
    }
}
