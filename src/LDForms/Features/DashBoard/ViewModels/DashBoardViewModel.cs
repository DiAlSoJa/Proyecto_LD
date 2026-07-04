using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Enums;
using LD.FormsX.Helpers;
using LD.FormsX.Movimientos;
using LD.FormsX.Views;
using LD.FormsX.Views.ASN;
using LD.FormsX.Views.Auditar;
using LD.FormsX.Features.Embarques.Views;
using LD.FormsX.Views.Catalogos;
using LD.FormsX.Views.CheckList;
using LD.FormsX.Views.ControlPatio;
using LD.FormsX.Views.DatabaseDiagram;
using LD.FormsX.Features.Surtidos.Views;
using LD.FormsX.Views.Inventario;
using LD.FormsX.Views.InventarioAleatorio;
using LD.FormsX.Views.Proyectos;
using LD.FormsX.Views.ReporteDanos;
using LD.FormsX.Views.Reportes;
using LD.FormsX.Views.Tareas;
using LD.FormsX.Views.Usuarios;
using LDForms.Features.DockDelivery.Views;
using LDForms.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace LDForms.Features.DashBoard.ViewModels;

public partial class DashBoardViewModel : ObservableObject
{
    private const string DashboardIconColor = "#233167";
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, ModuleMetadata> _modules;

    public DashBoardViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _modules = new Dictionary<string, ModuleMetadata>(StringComparer.OrdinalIgnoreCase)
        {
            ["Clientes"] = new("Clientes", "AccountGroupOutline", DashboardIconColor, Module_e.Clients, null, () => _serviceProvider.GetRequiredService<CatalogosClientesView>()),
            ["Proyectos"] = new("Proyectos", "ClipboardListOutline", DashboardIconColor, Module_e.Projects, null, () => _serviceProvider.GetRequiredService<ProyectosView>()),
            ["Almacenes"] = new("Almacén", "Warehouse", DashboardIconColor, Module_e.Warehouses, null, () => _serviceProvider.GetRequiredService<AlmacenesView>()),
            ["Ubicaciones"] = new("Ubicaciones", "MapMarkerOutline", DashboardIconColor, Module_e.Locations, null, () => _serviceProvider.GetRequiredService<UbicacionesView>()),
            ["Articulos"] = new("Artículos", "CubeOutline", DashboardIconColor, Module_e.Products, null, () => _serviceProvider.GetRequiredService<ArticulosView>()),
            ["Inventario"] = new("Inventario", "ClipboardCheckOutline", DashboardIconColor, Module_e.Inventory, null, () => _serviceProvider.GetRequiredService<InventarioView>()),
            ["Movimientos"] = new("Movimientos", "SwapHorizontal", DashboardIconColor, Module_e.Movements, null, () => _serviceProvider.GetRequiredService<MovimientosView>()),
            ["ASN"] = new("ASN", "PackageVariantClosed", DashboardIconColor, Module_e.ASN, null, () => _serviceProvider.GetRequiredService<ASNView>()),
            ["Auditar"] = new("Auditar", "ClipboardSearchOutline", DashboardIconColor, Module_e.Auditing, null, () => _serviceProvider.GetRequiredService<AuditarView>()),
            ["Aleatorio"] = new("Inventario aleatorio", "ShuffleVariant", DashboardIconColor, Module_e.RandomInventory, null, () => _serviceProvider.GetRequiredService<InventarioCiclicoView>()),
            ["Usuarios"] = new("Usuarios", "AccountMultipleOutline", DashboardIconColor, Module_e.Users, null, () => _serviceProvider.GetRequiredService<UsuariosView>()),
            ["CheckList"] = new("Checklist montacargas", "Forklift", DashboardIconColor, Module_e.ForkliftChecklist, null, () => _serviceProvider.GetRequiredService<CheckListView>()),
            ["Catalogos"] = new("Catálogos", "ViewGridOutline", DashboardIconColor, Module_e.Catalogs, null, () => _serviceProvider.GetRequiredService<CatalogosView>()),
            ["Surtido"] = new("Surtido", "ClipboardArrowDownOutline", DashboardIconColor, Module_e.Picking, null, () => _serviceProvider.GetRequiredService<SurtidosView>()),
            ["Embarques"] = new("Embarques", "ClipboardArrowDownOutline", DashboardIconColor, Module_e.Picking, null, () => _serviceProvider.GetRequiredService<EmbarquesView>()),
            ["Reportes"] = new("Reportes", "ChartBar", DashboardIconColor, Module_e.Reports, null, () => _serviceProvider.GetRequiredService<ReportesView>()),
            ["DatabaseDiagram"] = new("Diagrama BD", "Database", DashboardIconColor, Module_e.Reports, null, () => _serviceProvider.GetRequiredService<DatabaseDiagramView>()),
            ["ReporteDanos"] = new("Reporte de daños", "AlertCircleOutline", DashboardIconColor, Module_e.DamageReport, null, () => _serviceProvider.GetRequiredService<DamageReportView>()),
            ["Tareas"] = new("Tareas", "ClipboardSearchOutline", DashboardIconColor, Module_e.WarehouseStaff, null, () => _serviceProvider.GetRequiredService<TasksView>()),
            ["DockDelivery"] = new("Dock delivery", "TruckFlatbed", DashboardIconColor, Module_e.Operations, null, () => _serviceProvider.GetRequiredService<DockDeliveryView>()),
            ["Impresion"] = new("Impresión", "PrinterOutline", DashboardIconColor, Module_e.Reports, StandardLabelPrintOptionsDialog.StandardIdOption, null),
            ["Patio"] = new("Control de patio", "Parking", DashboardIconColor, Module_e.YardControl, null, () => _serviceProvider.GetRequiredService<ControlPatioView>())
        };

        Tiles = new ObservableCollection<ModuleTileVm>();

        foreach (var pair in _modules)
        {
            var key = pair.Key;
            var meta = pair.Value;

            var openCommand = new AsyncRelayCommand(() => ExecuteOpenAsync(key));
            var openInWindowCommand = new RelayCommand(() => ExecuteOpenInWindow(key));

            Tiles.Add(new ModuleTileVm(
                key,
                meta.Title,
                meta.IconKind,
                meta.IconColor,
                meta.Module,
                key.Equals("Impresion", StringComparison.OrdinalIgnoreCase),
                meta.PrintMenuHeader,
                openCommand,
                openInWindowCommand)
            {
                IsVisible = key.Equals("DockDelivery", StringComparison.OrdinalIgnoreCase)
                    ? HasDockDeliveryAccess()
                    : UserData.HasModule((int)meta.Module)
            });
        }

        TilesView = CollectionViewSource.GetDefaultView(Tiles);
        TilesView.Filter = FilterTile;
    }

    public ObservableCollection<ModuleTileVm> Tiles { get; }

    public ICollectionView TilesView { get; }

    [ObservableProperty]
    private string searchText = string.Empty;

    public event EventHandler<DashboardNavigationRequestedEventArgs>? NavigationRequested;

    private bool FilterTile(object item)
    {
        if (item is not ModuleTileVm tile)
        {
            return false;
        }

        if (!tile.IsVisible)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            return true;
        }

        var query = SearchText.Trim();
        return tile.Key.Contains(query, StringComparison.OrdinalIgnoreCase)
            || tile.Title.Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    partial void OnSearchTextChanged(string value)
    {
        TilesView.Refresh();
    }

    private async Task ExecuteOpenAsync(string key)
    {
        if (!TryGetMetadata(key, out var meta))
        {
            return;
        }

        if (key.Equals("Impresion", StringComparison.OrdinalIgnoreCase))
        {
            await PrintStandardLabelsAsync();
            return;
        }

        if (meta.ViewFactory is null)
        {
            return;
        }

        NavigationRequested?.Invoke(this, new DashboardNavigationRequestedEventArgs(meta.Title, meta.ViewFactory(), DashboardOpenMode.Tab));
    }

    private void ExecuteOpenInWindow(string key)
    {
        if (!TryGetMetadata(key, out var meta) || meta.ViewFactory is null)
        {
            return;
        }

        NavigationRequested?.Invoke(this, new DashboardNavigationRequestedEventArgs(meta.Title, meta.ViewFactory(), DashboardOpenMode.Window));
    }

    private bool TryGetMetadata(string key, out ModuleMetadata metadata)
    {
        return _modules.TryGetValue(key, out metadata!);
    }

    private static bool HasDockDeliveryAccess()
    {
        return UserData.HasPermission(PermissionKeys.Vehicle_View)
            || UserData.HasPermission(PermissionKeys.Vehicle_Create)
            || UserData.HasPermission(PermissionKeys.Vehicle_Update)
            || UserData.HasPermission(PermissionKeys.Vehicle_Delete);
    }

    private async Task PrintStandardLabelsAsync()
    {
        var option = ShowPrintOptionDialog();
        if (option != StandardLabelPrintOptionsDialog.StandardIdOption)
        {
            return;
        }

        var quantity = ShowQuantityDialog();
        if (!quantity.HasValue)
        {
            return;
        }

        try
        {
            var service = _serviceProvider.GetRequiredService<StandardLabelService>();
            var response = await service.GenerateStandardIds(quantity.Value);

            if (response.IsFailure || response.Data == null || response.Data.Count == 0)
            {
                DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudieron generar los StandardId.");
                return;
            }

            StandardIdLabelPrinter.PrintLabels(response.Data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    private string? ShowPrintOptionDialog()
    {
        var dialog = new StandardLabelPrintOptionsDialog();

        return dialog.ShowDialog() == true
            ? dialog.SelectedOption
            : null;
    }

    private int? ShowQuantityDialog()
    {
        var dialog = new StandardLabelQuantityDialog();

        return dialog.ShowDialog() == true ? dialog.Quantity : null;
    }

    private sealed record ModuleMetadata(
        string Title,
        string IconKind,
        string IconColor,
        Module_e Module,
        string? PrintMenuHeader,
        Func<UserControl>? ViewFactory);
}

public sealed class DashboardNavigationRequestedEventArgs : EventArgs
{
    public DashboardNavigationRequestedEventArgs(string title, UserControl view, DashboardOpenMode mode)
    {
        Title = title;
        View = view;
        Mode = mode;
    }

    public string Title { get; }

    public UserControl View { get; }

    public DashboardOpenMode Mode { get; }
}

public enum DashboardOpenMode
{
    Tab,
    Window
}
