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
            Routing.RegisterRoute(nameof(ReceptionDetailPage), typeof(ReceptionDetailPage));
            Routing.RegisterRoute(nameof(MovementPage), typeof(MovementPage));
            Routing.RegisterRoute(nameof(MovementDetail), typeof(MovementDetail));
            Routing.RegisterRoute(nameof(DamageReportPage), typeof(DamageReportPage));
            Routing.RegisterRoute(nameof(DamageReportDetailPage), typeof(DamageReportDetailPage));
            Routing.RegisterRoute(nameof(DamageReportPrintPage), typeof(DamageReportPrintPage));
            Routing.RegisterRoute(nameof(ReceptionPage), typeof(ReceptionPage));
            Routing.RegisterRoute(nameof(PickingPage), typeof(PickingPage));
            Routing.RegisterRoute(nameof(PickingKittingPage), typeof(PickingKittingPage));
            Routing.RegisterRoute(nameof(TaskList), typeof(TaskList));
            Routing.RegisterRoute(nameof(NewTask), typeof(NewTask));
            Routing.RegisterRoute(nameof(TaskResolve), typeof(TaskResolve));
            Routing.RegisterRoute(nameof(WarehouseOperations), typeof(WarehouseOperations));
            Routing.RegisterRoute(nameof(InventoryList), typeof(InventoryList));
            Routing.RegisterRoute(nameof(ForkliftChecklistPage), typeof(ForkliftChecklistPage));
            Routing.RegisterRoute(nameof(NoEquipmentPage), typeof(NoEquipmentPage));

            // Control de Patio
            Routing.RegisterRoute(nameof(PatioPendientesPage), typeof(PatioPendientesPage));
            Routing.RegisterRoute(nameof(PatioDetallePage),    typeof(PatioDetallePage));
            Routing.RegisterRoute(nameof(CortinaSeleccionPage), typeof(CortinaSeleccionPage));

            // Task Manager Seguridad
            Routing.RegisterRoute(nameof(TaskSecurityPage), typeof(TaskSecurityPage));
        }
    }
}
