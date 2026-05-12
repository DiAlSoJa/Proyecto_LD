# Copilot Instructions — LD.FormsX (WPF Desktop Client)

## Project Description
WPF desktop client (net10.0-windows) for back-office and administrative operations. Uses CommunityToolkit.Mvvm, MaterialDesignThemes, and Microsoft.Extensions.Hosting. Primary desktop client — replacing the legacy LD.Forms project.

## Architectural Constraints

- MVVM with `CommunityToolkit.Mvvm` — logic in ViewModels, not in XAML code-behind.
- Code-behind only handles: setting ViewModel, calling async init from `Loaded` event, hardware-specific events.
- Every new ViewModel and View/Window must be registered with `AddTransient` in `App.xaml.cs → RegisterServices`.
- All API calls through `LD.Client` services — never add `HttpClient` here.
- Do not reference `LD.Domain`, `LD.Application`, or `LD.Infrastructure`.
- Use `DialogHelper` for user feedback (ShowWarning, ShowError, ShowInfo).
- Use `App.Services.GetRequiredService<T>()` to resolve dialog windows.

## ViewModel Template

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.FormsX.Helpers;
using System.Collections.ObjectModel;

namespace LD.FormsX.Features.Foo.ViewModels;

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

    [RelayCommand(CanExecute = nameof(CanCreate))]
    private async Task CreateAsync()
    {
        var dialog = App.Services.GetRequiredService<NuevoFooView>();
        dialog.ShowDialog();
        await CargarDatosAsync();
    }
}
```

## View Code-Behind Template

```csharp
// Features/Foo/Views/FooView.xaml.cs
public partial class FooView : UserControl
{
    private readonly FooViewModel _vm;

    public FooView(FooViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = vm;
        Loaded += async (_, _) => await vm.CargarDatosAsync();
    }
}
```

## Register in App.xaml.cs

```csharp
// In RegisterServices():
services.AddTransient<FooViewModel>();
services.AddTransient<FooView>();
services.AddTransient<NuevoFooView>();
```

## Dialog Pattern

```csharp
// To show a dialog and refresh:
var dialog = App.Services.GetRequiredService<NuevoFooView>();
dialog.ShowDialog();
await CargarDatosAsync();
```

## Permission Checking in XAML

```xml
<Button Content="Nuevo"
        IsEnabled="{Binding CanCreate}"
        Command="{Binding CreateCommand}" />
```

## File/Folder Structure

```
Features/<Feature>/
├── Views/<Feature>View.xaml            ← Main list/display view
├── Views/<Feature>View.xaml.cs         ← Sets DataContext, calls init
├── Views/Nuevo<Feature>View.xaml       ← Create/edit dialog
├── Views/Nuevo<Feature>View.xaml.cs
└── ViewModels/<Feature>ViewModel.cs
└── ViewModels/Nuevo<Feature>ViewModel.cs
```

## Naming Conventions

- ViewModel: `<Feature>ViewModel` → `CatalogoCategoriasViewModel`, `NuevaCategoriaViewModel`
- View: `<Feature>View` or `Nuevo<Feature>View`
- Data loading method: `CargarDatosAsync()` (convention observed throughout)
- Permission properties: `CanCreate`, `CanEdit`, `CanView`

## What to Avoid

- Never add API/HTTP code to ViewModels — use `LD.Client` services
- Never forget to register new ViewModels and Views in `App.xaml.cs`
- Never handle data loading in the constructor — use `CargarDatosAsync()` called from `Loaded`
- Never reference `LD.Domain` or `LD.Infrastructure`
- Features/Embarques and Features/Surtido are stubs — do not generate code for them without confirming
