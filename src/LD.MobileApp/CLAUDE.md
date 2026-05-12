# LD.MobileApp — Project Guide for Claude Code

## Project Overview

`LD.MobileApp` is a .NET MAUI multi-platform mobile application (net10.0) targeting Android, iOS, macCatalyst, and Windows. It is the primary interface for warehouse field operations (reception, picking, scanning, checklist, security/patio, movements, damage reports).

The app uses **MVVM with CommunityToolkit.Mvvm** for all screens. Navigation is handled by **MAUI Shell** via `AppShell.xaml`.

## Structure

```
LD.MobileApp
├── App.xaml / App.xaml.cs          ← Application entry; sets AppShell as MainPage
├── AppShell.xaml                   ← Shell navigation routes
├── MauiProgram.cs                  ← DI configuration, service registration
├── Platforms/
│   ├── Android/                    ← Android-specific entry points and permissions
│   ├── iOS / MacCatalyst           ← iOS/Mac entry points
│   └── Windows/                    ← Windows entry point
├── Features/
│   ├── Auth/
│   │   ├── Views/LoginPage.xaml    ← Login screen
│   │   └── ViewModels/LoginPageViewModel.cs (LoginViewModel)
│   ├── Dashboard/
│   │   ├── Views/DashboardPage.xaml
│   │   └── ViewModels/DashboardViewModel.cs
│   ├── Almacenista/                ← Warehouse operations: reception, picking, change location
│   ├── Checklist/                  ← Forklift inspection checklist (ForkliftChecklistPage)
│   ├── Consultas/                  ← Movement queries
│   ├── Controls/                   ← Shared controls (Card, LoaderSpinner)
│   ├── Dashboard/
│   ├── Inventario/                 ← Inventory list view
│   ├── Operaciones/                ← Warehouse operations selection
│   ├── ReporteDanos/               ← Damage reports
│   ├── Scanning/                   ← Reusable scan pages (ScanStandarLD, Scan3Fields)
│   ├── Seguridad/                  ← Security/Patio: vehicle registration, driver sign, tasks, cortina selection
│   └── Tareas/                     ← Task management (list, new, resolve)
├── Converters/                     ← BoolToColorConverter
├── Models/                         ← ChecklistOption, ChecklistSection (UI-only models)
├── Resources/
│   ├── AppIcon / Splash / Images / Fonts / Raw
│   └── Styles/
└── Services/
    ├── ILoaderService.cs / NotificationItem.cs / NotificationsPopup.cs
    ├── OptionPopup.cs / OptionsToBoolConverter.cs / ReadDotColorConverter.cs
    └── ILoaderService (LoaderSpinner)
```

## MVVM Pattern

### ViewModel Convention

All ViewModels extend `ObservableObject` (CommunityToolkit.Mvvm):

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class FooViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    private readonly FooService _fooService;

    public FooViewModel(FooService fooService)
    {
        _fooService = fooService;
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsLoading = true;
        try
        {
            var result = await _fooService.GetFooAsync();
            if (!result.IsSuccess)
            {
                await Shell.Current.DisplayAlertAsync("Error", result.Message, "OK");
                return;
            }
            // update properties
        }
        finally
        {
            IsLoading = false;
        }
    }
}
```

### Page Convention

Pages receive their ViewModel via constructor injection and set `BindingContext`:

```csharp
public partial class FooPage : ContentPage
{
    public FooPage(FooViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

### Navigation

Uses MAUI Shell:
- Route registration in `AppShell.xaml` or `AppShell.xaml.cs`
- Navigation: `Shell.Current.GoToAsync(nameof(FooPage))` or `Shell.Current.GoToAsync("//route")`
- Pass parameters: `Shell.Current.GoToAsync(nameof(FooPage), new Dictionary<string, object> { { "Key", value } })`

## Key Features

### Login Flow

`LoginViewModel` calls `AuthService.LoginAsync()`, stores tokens in `UserSession`, calls `GetMeAsync()` to populate `UserData`, then checks if the user has an assigned equipment. If yes, navigates to `ForkliftChecklistPage`. If no, navigates to `//dashboard`.

### Forklift Checklist

`ForkliftChecklistViewModel` uses:
- `EquipmentQuestionService` to load questions by equipment type
- `Plugin.Maui.OCR` to scan horómetro (hourmeter) from a camera photo
- `ChecklistService` to submit the completed checklist and upload photos

### Scanning

`ScanStandarLD` and `Scan3Fields` are reusable scanning pages using **ZXing.Net.Maui** for barcode reading.

### Security / Patio

Covers vehicle registration, driver signature, cortina (gate) assignment, and task management for the yard control workflow.

## Hardware / Platform Features

| Capability | Package |
|---|---|
| Barcode scanning | `ZXing.Net.Maui 0.7.4` + `ZXing.Net.Maui.Controls` |
| OCR (horómetro) | `Plugin.Maui.OCR 1.1.1` |
| Photo capture | `MediaPicker.CapturePhotoAsync()` (MAUI built-in) |
| Preferences storage | `Preferences.Default` (MAUI built-in) — used for API base URL |

## DI Registration

In `MauiProgram.cs` (not shown but implied), services are registered using:

```csharp
services.AddLDClient(options =>
{
    options.BaseUrl = Preferences.Default.Get("ApiBaseUrl", "http://192.168.0.103:8050/api");
});

// Register ViewModels and Pages
services.AddTransient<LoginViewModel>();
services.AddTransient<LoginPage>();
// etc.
```

## Default API URL

Configured at runtime via `Preferences.Default.Get("ApiBaseUrl", "http://192.168.0.103:8050/api")`. Users can change it from the login screen via the IP config prompt.

## Dependencies

**Project references:** `LD.Client`, `LD.Contracts`

**NuGet:** `CommunityToolkit.Maui 8.*`, `CommunityToolkit.Mvvm 7.1.2`, `ZXing.Net.Maui 0.7.4`, `Plugin.Maui.OCR 1.1.1`, `Refractored.MvvmHelpers 1.6.2`, `SkiaSharp 2.88.*`, `Microsoft.Maui.Controls 10.0.41`

## Building

```bash
# Android (most common for development)
dotnet build src/LD.MobileApp/LD.MobileApp.csproj -f net10.0-android

# Windows (for testing on desktop)
dotnet build src/LD.MobileApp/LD.MobileApp.csproj -f net10.0-windows10.0.19041.0
```

## Common Tasks

1. **Add a new screen**: Create `Features/<Feature>/Views/<Screen>Page.xaml` + `Features/<Feature>/ViewModels/<Screen>ViewModel.cs`. Inherit `ContentPage` and `ObservableObject`. Register both in DI. Add route to `AppShell`.
2. **Add a camera/scan operation**: Use `MediaPicker` or inject `ZXing` scanner; handle in ViewModel.
3. **Add an API call**: Use the relevant `LD.Client` service method — do not create HTTP calls directly.

## Things Claude Must NOT Do

- Do not add HTTP calls directly — use `LD.Client` services
- Do not reference `LD.Domain`, `LD.Application`, or `LD.Infrastructure`
- Do not put business logic in Views (code-behind should only set BindingContext)
- Do not create platform-specific code unless absolutely required for the feature
- Do not change the namespace `MauiAppLogin` — it is used throughout (legacy root namespace)
