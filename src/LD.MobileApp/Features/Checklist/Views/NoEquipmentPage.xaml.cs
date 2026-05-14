using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class NoEquipmentPage : ContentPage
{
    public NoEquipmentPage(NoEquipmentViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
