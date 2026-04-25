namespace MauiAppLogin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
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
            Routing.RegisterRoute(nameof(TaskList), typeof(TaskList));
            Routing.RegisterRoute(nameof(NewTask), typeof(NewTask));
            Routing.RegisterRoute(nameof(TaskResolve), typeof(TaskResolve));
            Routing.RegisterRoute(nameof(WarehouseOperations), typeof(WarehouseOperations));
            Routing.RegisterRoute(nameof(InventoryList), typeof(InventoryList));
            Routing.RegisterRoute(nameof(ForkliftChecklistPage), typeof(ForkliftChecklistPage));

            // Control de Patio
            Routing.RegisterRoute(nameof(PatioPendientesPage), typeof(PatioPendientesPage));
            Routing.RegisterRoute(nameof(PatioDetallePage),    typeof(PatioDetallePage));
            Routing.RegisterRoute(nameof(CortinaSeleccionPage), typeof(CortinaSeleccionPage));
        }
    }
}
