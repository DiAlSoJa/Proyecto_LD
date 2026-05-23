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
using MauiAppLogin.Controls;

public partial class FooViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    private readonly FooService _fooService;
    private readonly IDialogService _dialogService;

    public FooViewModel(FooService fooService, IDialogService dialogService)
    {
        _fooService = fooService;
        _dialogService = dialogService;
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
                await _dialogService.ShowErrorAsync("Error", result.Message);
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

### Status Dialogs — IDialogService (MANDATORY)

**Never use `DisplayAlertAsync`** for status messages. Always inject and use `IDialogService`:

| Method | When to use |
|---|---|
| `ShowErrorAsync(title, msg)` | API errors, validation failures, exceptions |
| `ShowSuccessAsync(title, msg)` | Successful save, operation completed |
| `ShowInfoAsync(title, msg)` | Neutral information, instructions |
| `ShowWarningAsync(title, msg)` → `bool` | Two-option confirmation (returns `true` = Continuar) |
| `ShowBlocking(title, msg)` / `HideBlocking()` | Mandatory blocking state (e.g. pending checklist) |

`DisplayPromptAsync` remains valid for capturing text input. `LogoutDialog` and `OptionPopup` are specialized UX popups — use them directly as needed.

### Form Entry Fields — LabeledEntry / PasswordEntry (MANDATORY)

**Never use `<Entry>` inside a manual `<Border>`** in forms. Always use the custom controls in `Features/Controls/`:

- **`<controls:LabeledEntry>`** — for plain text fields. Namespace: `xmlns:controls="clr-namespace:MauiAppLogin.Views.Controls"`.
- **`<controls:PasswordEntry>`** — for password fields (uses `Password` property instead of `Text`).

```xaml
<!-- Correct -->
<controls:LabeledEntry
    LabelText="Nombre:"
    Text="{Binding Nombre}"
    Placeholder="Ej. Carlos Ramírez"/>

<!-- Wrong — standard Entry inside manual Border -->
<VerticalStackLayout>
    <Label Text="Nombre:"/>
    <Border><Entry Text="{Binding Nombre}"/></Border>
</VerticalStackLayout>
```

**`LabeledEntry` BindableProperties**: `LabelText` (string), `Text` (string, TwoWay), `Placeholder` (string), `IconSource` (ImageSource?).

### Photo Controls — PhotoGallery / PhotoThumb / ImagePreviewPopup (MANDATORY)

**Never use `CollectionView HorizontalList` or manual thumbnail grids** for captured photos. Use the controls in `Features/Controls/`:

| Situation | Control |
|---|---|
| One or more photos (scrollable gallery) | `<controls:PhotoGallery>` |
| Single photo item with delete icon | `<controls:PhotoThumb>` directly |
| Full-screen image preview on tap | `ImagePreviewPopup` from ViewModel |

```xaml
<controls:PhotoGallery
    ItemsSource="{Binding Photos}"
    DeleteCommand="{Binding RemovePhotoCommand}"
    ViewCommand="{Binding ViewPhotoCommand}"
    MinimumHeightRequest="90"/>
```

```csharp
// ViewPhotoCommand pattern in ViewModel (synchronous Command<T>)
ViewPhotoCommand = new MvvmHelpers.Commands.Command<TPhotoItem>(item =>
{
    if (item?.Source is null) return;
    (Shell.Current.CurrentPage ?? Application.Current?.MainPage)
        ?.ShowPopup(new ImagePreviewPopup(item.Source));
});
```

**`PhotoGallery` BindableProperties**: `ItemsSource` (IEnumerable), `DeleteCommand` (ICommand), `ViewCommand` (ICommand).  
**`PhotoThumb` BindableProperties**: `PhotoSource` (ImageSource), `DeleteCommand`, `DeleteCommandParameter`, `ViewCommand`, `ViewCommandParameter`.  
Delete icon is a **trash can** (🗑) — never ✕ on photo thumbnails.

### Mandatory Review Rule

When reading or editing **any** Page or ViewModel in this project, if you find:
- `<Entry>` inside a manual `<Border>` → migrate to `<controls:LabeledEntry>` in the same edit.
- `DisplayAlert` / `DisplayAlertAsync` / `DisplayActionSheet` → migrate to `IDialogService` in the same edit.
- `CollectionView HorizontalList` or manual photo thumbnails → migrate to `<controls:PhotoGallery>` in the same edit.

Do not leave Pages with these patterns unmigrated when you touch them. There is a backlog of ~60 `DisplayAlertAsync` instances project-wide; migrate them incrementally as each file is edited.

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

### Authentication and Session Flow (Current)

- API auth uses JWT Bearer (`Issuer: LdProyectAPI`, `Audience: LdProyectClient`).
- `ApiService` is the single HTTP client instance registered by `AddLDClient`.
- After successful login (`AuthService.LoginAsync`), the app:
  1. Calls `ApiService.SetBearerToken(accessToken)`.
  2. Stores tokens in `UserSession`.
  3. Persists access/refresh/expiry in `SecureStorage` via `MobileSessionService.PersistAsync`.
  4. Calls `GetMe` and populates `UserData`.

`MobileSessionService` is configured at startup in `MauiProgram.cs` with the root `ApiService` + `AuthService` and wires:

- `apiService.OnUnauthorizedAsync = TryRefreshAsync`

This enables automatic refresh on 401 for normal JSON calls:

1. Any `GetAsync/PostAsync/PutAsync/DeleteAsync` gets 401.
2. `ApiService` runs `TryRefreshTokenAsync()`.
3. `MobileSessionService.TryRefreshAsync()` sends `POST /api/auth/refresh` with the current refresh token.
4. If refresh succeeds, new tokens are persisted, `UserSession` is updated, and `SetBearerToken(newAccessToken)` is applied.
5. Original request is retried once.

Concurrency behavior:

- `ApiService` serializes refresh using `SemaphoreSlim` and shares one refresh task across concurrent 401s.
- Parallel requests wait for the same in-flight refresh result instead of launching multiple refresh calls.
- A recursion guard prevents infinite loops if `/auth/refresh` itself returns 401.

Failure behavior:

- If refresh token is missing/invalid/expired, session is cleared (`SecureStorage` + `UserSession`) and `SessionExpiredMessage` is published.
- Multipart requests are not auto-retried after 401 because content streams may already be consumed.

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
