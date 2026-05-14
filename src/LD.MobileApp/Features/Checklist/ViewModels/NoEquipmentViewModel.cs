using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class NoEquipmentViewModel : ObservableObject
{
    public ICommand LogoutCommand { get; }

    public NoEquipmentViewModel()
    {
        LogoutCommand = new AsyncCommand(LogoutAsync);
    }

    private async Task LogoutAsync()
    {
        UserSession.LogOut();
        await Shell.Current.GoToAsync("//login");
    }
}
