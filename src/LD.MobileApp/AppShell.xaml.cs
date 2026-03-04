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
            Routing.RegisterRoute(nameof(MovementPage), typeof(MovementPage));
            Routing.RegisterRoute(nameof(MovementDetail), typeof(MovementDetail));
            Routing.RegisterRoute(nameof(DamageReportPage), typeof(DamageReportPage));
            Routing.RegisterRoute(nameof(DamageReportDetailPage), typeof(DamageReportDetailPage));
            Routing.RegisterRoute(nameof(DamageReportPrintPage), typeof(DamageReportPrintPage));
            Routing.RegisterRoute(nameof(ReceptionPage), typeof(ReceptionPage));
            Routing.RegisterRoute(nameof(PickingPage), typeof(PickingPage));
            Routing.RegisterRoute(nameof(TaskSecurity), typeof(TaskSecurity));
        }
    }
}
