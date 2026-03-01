namespace MauiAppLogin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(RegisterVehicule), typeof(RegisterVehicule));
            Routing.RegisterRoute(nameof(RegisterLicense), typeof(RegisterLicense));
            Routing.RegisterRoute(nameof(SignatureDriver), typeof(SignatureDriver));
            Routing.RegisterRoute(nameof(ChangeLocationPage), typeof(ChangeLocationPage));
        }
    }
}
