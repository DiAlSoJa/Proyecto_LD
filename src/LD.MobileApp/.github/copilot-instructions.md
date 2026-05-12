# Copilot Instructions — LD.MobileApp

## Project Description
.NET MAUI multi-platform mobile app (net10.0) for Android, iOS, macCatalyst, and Windows. Primary interface for field warehouse operations: reception, picking, scanning, forklift checklist, yard control, damage reports. Root namespace is `MauiAppLogin` (legacy).

## Architectural Constraints

- MVVM with `CommunityToolkit.Mvvm` — all logic in ViewModels, no logic in code-behind.
- Code-behind only sets `BindingContext` and handles MAUI-specific lifecycle (e.g., `Loaded` event for async init).
- Navigation uses MAUI Shell (`Shell.Current.GoToAsync(...)`).
- All API calls go through `LD.Client` services — never add `HttpClient` directly.
- Do not reference `LD.Domain`, `LD.Application`, or `LD.Infrastructure`.

## ViewModel Template

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;

namespace MauiAppLogin.ViewModels;

public partial class FooViewModel : ObservableObject
{
    private readonly FooService _fooService;

    [ObservableProperty]
    private ObservableCollection<FooDto> items = new();

    [ObservableProperty]
    private bool isLoading;

    public FooViewModel(FooService fooService)
    {
        _fooService = fooService;
    }

    [RelayCommand]
    private async Task LoadAsync()
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
            Items = new ObservableCollection<FooDto>(result.Data ?? []);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
```

## Page Template

```csharp
// Features/Foo/Views/FooPage.xaml.cs
public partial class FooPage : ContentPage
{
    public FooPage(FooViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

## Navigation

```csharp
// Simple navigation
await Shell.Current.GoToAsync(nameof(FooPage));

// With parameters (QueryProperty)
await Shell.Current.GoToAsync(nameof(FooPage),
    new Dictionary<string, object> { { "FooId", 42 } });

// Back to root tab
await Shell.Current.GoToAsync("//dashboard");
```

## Barcode Scanning (ZXing)

Use `ZXing.Net.Maui.Controls.CameraBarcodeReaderView` in XAML.

## OCR (horómetro)

```csharp
var photo = await MediaPicker.CapturePhotoAsync();
using var stream = await photo.OpenReadAsync();
using var ms = new MemoryStream();
await stream.CopyToAsync(ms);
var result = await OcrPlugin.Default.RecognizeTextAsync(ms.ToArray());
var match = Regex.Match(result.AllText, @"\d+[\.,]?\d*");
```

## File/Folder Structure

```
Features/<Feature>/
├── Views/<Screen>Page.xaml       ← XAML UI
├── Views/<Screen>Page.xaml.cs    ← Sets BindingContext only
├── ViewModels/<Screen>ViewModel.cs
└── Models/                       ← UI-only models (not domain entities)
```

## Naming Conventions

- Page class: `<Feature>Page` (e.g., `ForkliftChecklistPage`)
- ViewModel class: `<Feature>ViewModel` (e.g., `ForkliftChecklistViewModel`)
- Observable property: camelCase field → generates PascalCase property (e.g., `isLoading` → `IsLoading`)
- Relay command: method in camelCase → generates `<Name>Command` property

## What to Avoid

- Never put API calls or business logic in code-behind (`.xaml.cs`)
- Never add `HttpClient` or `HttpClientHandler` — use `LD.Client` services
- Never use `Application.Current.Dispatcher` for cross-thread — use `MainThread.BeginInvokeOnMainThread`
- Never reference `LD.Domain` or `LD.Infrastructure`
- The namespace `MauiAppLogin` is legacy — do not rename it; new files may use it
