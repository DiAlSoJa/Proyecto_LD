using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MauiAppLogin.Models
{
    public class ChecklistQuestion
    {
        public string Label { get; set; } = string.Empty;

        public ObservableCollection<string> Options { get; set; } = new();

        public string? SelectedOption { get; set; }
    }
}
