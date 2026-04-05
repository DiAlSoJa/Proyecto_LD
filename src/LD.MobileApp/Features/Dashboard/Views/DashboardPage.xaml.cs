using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel dashboardViewModel)
    {
        InitializeComponent();
        BindingContext = dashboardViewModel;
    }
}
