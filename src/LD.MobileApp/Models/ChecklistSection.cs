using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MauiAppLogin.Models
{
    public class ChecklistSection
    {
        public string Title { get; set; } = string.Empty;

        public ObservableCollection<ChecklistQuestion> Questions { get; set; } = new();
    }
}
