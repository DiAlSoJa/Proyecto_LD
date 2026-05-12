# LD.FormsX — Project Guide for Claude Code

## Project Overview

`LD.FormsX` (net10.0-windows) is the WPF desktop client for back-office and administrative operations. It targets warehouse managers and administrators who configure the WMS, manage users, set up scanning, manage catalog data, and monitor operations.

The app uses **MVVM with CommunityToolkit.Mvvm**, **MaterialDesignThemes** for UI, and **Microsoft.Extensions.Hosting** for DI/configuration. Navigation uses a custom window-based approach (not a router).

## Structure

```
LD.FormsX (project root: src/LDForms)
├── App.xaml / App.xaml.cs          ← Host setup, DI registration, Serilog, startup
├── AssemblyInfo.cs
├── Controls/                       ← Reusable WPF controls (ActionButton, LoadingOverlay, StyledDataGrid)
├── Core/Enums/DialogType.cs
├── Features/
│   ├── Almacen/                    ← Warehouses (AlmacenesView + NuevoAlmacenView + ViewModels)
│   ├── Articulos/                  ← Products/articles (ArticulosView, NuevoArticuloView, CargaMasivaArticulosView)
│   ├── ASN/                        ← ASN management, scan view, vehicle search, label printing
│   ├── Auditar/                    ← Auditing views
│   ├── Catalogos/
│   │   ├── Categorias/             ← Category catalog (CatalogoCategoriasView + NuevaCategoriaView + ViewModels)
│   │   ├── Dimensionador/          ← Dimensioner catalog
│   │   ├── Equipos/                ← Equipment catalog
│   │   ├── Familias/               ← Family catalog
│   │   ├── Monedas/                ← Currency catalog
│   │   ├── Status/                 ← Inventory status catalog
│   │   └── Unidades/               ← Unit catalog
│   ├── CheckList/                  ← Checklist configuration (questions, equip assignment, summary, battery summary tabs)
│   ├── Clientes/                   ← Client management
│   ├── Common/                     ← Shared controls: DialogWindow, FilterableLookupComboBox, LookupPopupControl, DataGridNavigationManager, InlineLookupEditor
│   ├── ControlPatio/               ← Yard control view (ControlPatioView)
│   ├── DashBoard/                  ← Main dashboard + StandardLabel printing
│   ├── Embarques/                  ← Shipments (placeholder — Class1.cs stubs only)
│   ├── Inventario/                 ← Inventory view with dialogs for location/status/warehouse change
│   ├── InventarioAleatorio/        ← Cyclic inventory
│   ├── Login/                      ← Login screen and ViewModel
│   ├── Movimientos/                ← Inventory movements view
│   ├── Proyectos/                  ← Projects (ProyectosView + NuevoProyectoView + ViewModels)
│   ├── Reportes/                   ← Reports view
│   ├── Shared/                     ← MessageDialog, ToastNotification
│   ├── Surtido/                    ← Picking (placeholder — Class1.cs stubs only)
│   ├── Ubicaciones/                ← Locations (UbicacionesView + NuevaUbicacionView + NuevaUbicacionMasivaView + ViewModels)
│   └── Usuarios/                   ← Users + Roles management
├── Helpers/
│   ├── DialogHelper.cs / ToastHelper.cs   ← Show modal dialogs and toast notifications
│   ├── DataGridColumnFilterManager.cs
│   ├── DataGridExportExtensions.cs
│   ├── DataGridFilterStyler.cs
│   └── WpfGridFilter.cs
├── Model/LookupItem.cs
├── Resources/Images / Styles / Fonts
└── appsettings.json / appsettings.Development.json
```

## MVVM Pattern

### ViewModel Convention

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class FooViewModel : ObservableObject
{
    private readonly FooService _fooService;

    [ObservableProperty]
    private ObservableCollection<FooDto> items = new();

    [ObservableProperty]
    private FooDto? selectedItem;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    public FooViewModel(FooService fooService)
    {
        _fooService = fooService;
        CanCreate = UserData.HasPermission(PermissionKeys.Foo_Create);
        CanEdit   = UserData.HasPermission(PermissionKeys.Foo_Update);
    }

    public async Task CargarDatosAsync()
    {
        var result = await _fooService.GetFooAsync();
        if (!result.IsSuccess)
        {
            DialogHelper.ShowWarning(result.Message);
            return;
        }
        Items = new ObservableCollection<FooDto>(result.Data ?? []);
    }

    [RelayCommand]
    private async Task CreateAsync() { /* ... */ }
}
```

### View Convention

Views are WPF UserControls (XAML + code-behind). The code-behind only handles:
- Setting the ViewModel (received via DI)
- Calling async init methods (`Loaded` event)
- Hardware/UI-specific operations (e.g., calling `CargarDatosAsync`)

### Dialog Pattern

Dialogs are new `Window` instances created via `App.Services.GetRequiredService<NuevoFooView>()`:

```csharp
// In ViewModel or View code-behind:
var dialog = App.Services.GetRequiredService<NuevoFooView>();
dialog.ShowDialog();
await CargarDatosAsync(); // refresh after dialog closes
```

`DialogHelper` provides static helpers: `ShowWarning(msg)`, `ShowError(msg)`, `ShowInfo(msg)`.

## Host and DI Setup

`App.xaml.cs` uses `Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()`:
- Reads `appsettings.json` and `appsettings.{env}.json` (environment from `DOTNET_LD_ENVIRONMENT`)
- Configures Serilog (rolling file in `logs/`)
- Calls `RegisterServices(services, configuration)` which:
  - Calls `services.AddLDClient(...)` to register all HTTP services
  - Registers every ViewModel and View/Window with `AddTransient`

**Every new ViewModel and View must be registered in `RegisterServices`.**

## Navigation

There is no router. Navigation works by:
1. After login: `LoginViewModel` calls `_serviceProvider.GetRequiredService<DashBoard>()` and calls `.Show()` and `CloseAction?.Invoke()` (closes login window).
2. The `DashBoard` hosts a navigation menu that loads different views into a content area.

## Permission Checking in UI

Check permissions in the ViewModel constructor:

```csharp
CanCreate = UserData.HasPermission(PermissionKeys.Foo_Create);
```

Bind to `CanCreate` in XAML to show/hide buttons:

```xml
<Button Content="Nuevo" IsEnabled="{Binding CanCreate}" />
```

## DataGrid Helpers

`DataGridColumnFilterManager` — adds per-column filtering to WPF DataGrids.
`DataGridExportExtensions` — export DataGrid data.
`DataGridFilterStyler` — applies consistent filter styles.
`WpfGridFilter` — filter logic.

## Configuration

`appsettings.json`:
- `ApiSettings:BaseUrl` — e.g., `http://192.168.0.112:8050/api`

`appsettings.Development.json` — overrides for development (local API URL).

## Dependencies

**Project references:** `LD.Client`, `LD.Contracts`

**NuGet:** `CommunityToolkit.Mvvm 8.4.2`, `MaterialDesignThemes 5.3.1`, `MaterialDesignColors 5.3.1`, `Microsoft.Extensions.Hosting 10.0.3`, `Serilog 4.3.1`, `QRCoder 1.6.0`

## Building and Running

```bash
dotnet build src/LDForms/LD.FormsX.csproj
dotnet run --project src/LDForms/LD.FormsX.csproj
```

Set `DOTNET_LD_ENVIRONMENT=Development` to use `appsettings.Development.json`.

## Common Tasks

1. **Add a new catalog screen**: Create `Features/Catalogos/<Name>/Views/<Name>View.xaml` + `<Name>View.xaml.cs`, `Views/Nuevo<Name>View.xaml` + `.cs`, `ViewModels/<Name>ViewModel.cs` + `Nuevo<Name>ViewModel.cs`. Register all in `App.xaml.cs`.
2. **Show a dialog**: Use `App.Services.GetRequiredService<NuevoFooView>().ShowDialog()`.
3. **Display a toast/warning**: Use `DialogHelper.ShowWarning(msg)` or `ToastHelper`.
4. **Add a new API call**: Use the injected feature service from `LD.Client`; never add HTTP code here.

## Things Claude Must NOT Do

- Do not add HTTP calls directly — use `LD.Client` services
- Do not reference `LD.Domain`, `LD.Application`, or `LD.Infrastructure`
- Do not forget to register new ViewModels and Views in `App.xaml.cs → RegisterServices`
- Do not put business logic in code-behind — keep it in ViewModels
- Features/Embarques and Features/Surtido are currently stub/placeholder — do not assume they are complete
