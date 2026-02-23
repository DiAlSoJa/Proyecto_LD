using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiAppLogin.ViewModels
{
    public partial class BasesViewModel:ObservableObject
    {
        [ObservableProperty]
        public bool _isBussy;
        [ObservableProperty]
        public string _title;   
    }
}
