# Copilot Instructions — LD Solution (Root)

## Solution Overview

**Proyecto_LD** is a multi-client WMS (Warehouse Management System). A single ASP.NET Core API serves three frontends:
- `LD.MobileApp` — .NET MAUI for field operations (Android/iOS/Windows)
- `LD.FormsX` — WPF desktop for back-office administration
- `LD.Forms` — Legacy WinForms client (low priority, bug-fixes only)

**Tech stack:** .NET 9 / .NET 10, SQL Server Express, EF Core, ASP.NET Core Identity, JWT, MediatR, AutoMapper, FluentValidation, CommunityToolkit.Mvvm, MaterialDesignThemes, Serilog.

## Project Dependency Map

```
LD.MobileApp ──────────────────────────────────────────────┐
LD.FormsX  ─────────────────────────────────────────────── LD.Client ──── LD.Contracts
LD.Forms  ──────────────────────────────────────────────────────────────────────────┘
                                                                                    ↑
LD.Api ─────────── LD.Application ──── LD.Domain                                   │
         └──────── LD.Infrastructure ── LD.Domain          LD.Contracts ────────────┘
                                     └── LD.Application
```

Full dependency chains:
- `LD.Api` → `LD.Application` + `LD.Infrastructure` + `LD.Contracts`
- `LD.Application` → `LD.Domain` + `LD.Contracts`
- `LD.Infrastructure` → `LD.Domain` + `LD.Application` + `LD.Contracts`
- `LD.Client` → `LD.Contracts`
- `LD.MobileApp` → `LD.Client` + `LD.Contracts`
- `LD.FormsX` → `LD.Client` + `LD.Contracts`
- `LD.Forms` → `LD.Client` + `LD.Contracts`

## Data Flow for a Typical Feature

```
[MAUI ViewModel] or [WPF ViewModel]
    ↓ calls
[LD.Client/Services/<Feature>Service]
    ↓ HTTP POST/GET/PUT
[LD.Api/Controllers/<Feature>Controller]
    ↓ Mediator.Send(command/query)
[LD.Application/Features/<Feature>/Commands or Queries]
    ↓ IRepository<T>
[LD.Infrastructure/Repositories/<Feature>Repository]
    ↓ DbContext
[SQL Server]
```

## Solution-Wide Guidelines

### 1. Layer Responsibility (hard rule)
| Layer | What goes here |
|---|---|
| `LD.Domain` | Pure entity classes, enums, AuditableEntity base |
| `LD.Contracts` | DTOs, Requests, Responses, Enums, PermissionKeys |
| `LD.Application` | CQRS handlers, validators, AutoMapper profiles, interfaces |
| `LD.Infrastructure` | EF DbContext, repos, identity, JWT, interceptor |
| `LD.Api` | Controllers (routing only), authorization, middleware |
| `LD.Client` | Typed HTTP services, API URL registry, session state |
| `LD.MobileApp` | MAUI pages + ViewModels for mobile features |
| `LD.FormsX` | WPF views + ViewModels for desktop features |

### 2. Permissions
- **Source of truth**: `LD.Contracts/Constants/PermissionKeys.cs`
- Format: `"domain.action"` (e.g., `"categories.read"`, `"warehouse-staff.asn.execute"`)
- API: decorate with `[Permission(PermissionKeys.X_Y)]`
- Clients: check with `UserData.HasPermission(PermissionKeys.X_Y)`
- Adding a new permission: add the constant to `PermissionKeys.cs` + seed it in DB via migration

### 3. Response Envelope
- Server produces: `Result<T>` (in `LD.Application.Common.Results`)
- Client receives: `ApiResponseDto<T>` (in `LD.Contracts.Responses`)
- Both have: `IsSuccess`, `Data`, `Code`, `Message`, `Errors`
- The API serializes `Result<T>` and clients deserialize as `ApiResponseDto<T>` — they are structurally identical

### 4. Auditing (automatic)
`AuditableEntitySaveChangesInterceptor` fills `CreatedAt`, `CreatedByUserId`, `LastModifiedAt`, `LastModifiedByUserId` automatically for any entity inheriting `AuditableEntity`. **Never set these fields manually.**

### 5. EF Configuration
All entity configurations (Fluent API, indexes, relationships) are in `LdProyectDbContext.OnModelCreating()` in `LD.Infrastructure`. No `IEntityTypeConfiguration<T>` files.

### 6. Migrations
Always run with both `--project` and `--startup-project`:
```bash
dotnet ef migrations add <FeatureName> --project src/LD.Infrastructure --startup-project src/LD.Api
```
Migrations apply automatically on API startup.

### 7. Refresh Token / Authentication Flow (current)
- Changes to auth/refresh require explicit user approval before implementation.
- `LD.Client/Services/ApiService` is the shared HTTP base for MAUI + Forms and handles 401 retry flow.
- `ApiService` uses `OnUnauthorizedAsync` callback for refresh logic:
  - MAUI wires this callback from `MobileSessionService` in `MauiProgram.cs`.
  - Forms may leave it null (no auto-refresh) unless explicitly wired.
- On 401 from GET/POST/PUT/DELETE:
  1. client runs refresh callback,
  2. if refresh succeeds, applies new bearer token,
  3. retries original request once.
- Concurrent 401s are synchronized with a shared refresh task (single refresh in flight).
- Multipart requests are not retried automatically.

### 8. CORS
The API uses `AllowAll` CORS policy (any origin/method/header). This is intentional for the local network WMS deployment.

## Adding a Feature End-to-End

Follow this order strictly:

1. **Domain** (`LD.Domain/Entities/`): Add entity class extending `AuditableEntity`.
2. **Contracts** (`LD.Contracts/`): Add `FooRequest.cs` (input), `FooDto.cs` (output), permission constants in `PermissionKeys.cs`.
3. **Infrastructure** (`LD.Infrastructure/`): Add EF config in `LdProyectDbContext.OnModelCreating()`. Run `dotnet ef migrations add AddFoo`. Optionally create `FooRepository.cs` if custom queries are needed.
4. **Application** (`LD.Application/Features/Foo/`): Add `Commands/`, `Queries/`, `Validators/`, `Profiles/` folders and files.
5. **API** (`LD.Api/Controllers/`): Add `FooController.cs` inheriting `CommonController`.
6. **Client** (`LD.Client/Services/`): Add `FooService.cs`. Add URL properties to `ApiEndpoints.cs`. Register in `ConfigureServices.cs`.
7. **MAUI** (`LD.MobileApp/Features/Foo/`): Add ViewModel + Page. Register in DI. Add route to AppShell.
8. **WPF** (`src/LDForms/Features/Foo/`): Add ViewModel + Views. Register in `App.xaml.cs → RegisterServices`.

## Cross-Cutting Concerns

### Authentication
- JWT Bearer tokens, issuer: `LdProyectAPI`, audience: `LdProyectClient`
- After login: clients call `GetMe` to populate `UserData` (permissions, modules)
- `UserData.HasPermission(key)` and `UserData.HasModule(moduleId)` control UI visibility
- Access token lifetime: 8h (issued in `JwtTokenService`)
- Refresh token lifetime: 7 days, persisted and rotated in DB (`RefreshTokens`)
- MAUI persists access/refresh/expiry in `SecureStorage` via `MobileSessionService`
- `ApiService` must remain a single shared instance per app container to keep bearer and refresh callback state consistent

### Logging
- Server: Serilog → `logs/log-*.json` rolling daily + console
- WPF client: Serilog → `logs/log-*.txt` rolling daily
- MediatR pipeline: `LoggingBehavior` logs every handler name

### Validation
- `ValidationBehavior` runs all FluentValidation validators before every MediatR handler
- Returns `Result<string>.Failure(code: 400)` with the list of error messages if validation fails

### Session State (clients)
- `UserSession` (static): `AccessToken`, `RefreshToken`
- `UserData` (static): user identity + authorization tree (modules + permissions)
- `ApiService.SetBearerToken()` must be called after each successful login
- MAUI registers `MobileSessionService` and configures `apiService.OnUnauthorizedAsync = TryRefreshAsync` during startup
- On refresh failure in MAUI, session is cleared and `SessionExpiredMessage` is published

## Sensitive Files — Ask Before Changing

- `src/LD.Api/Program.cs`
- `src/LD.Infrastructure/Persistence/LdProyectDbContext.cs`
- `src/LD.Infrastructure/Persistence/Interceptors/AuditableEntitySaveChangesInterceptor.cs`
- `src/LD.Infrastructure/Services/Auth/JwtTokenService.cs`
- `src/LD.Api/Authorization/PermissionHandler.cs` / `PermissionService.cs`
- Any `appsettings.json` file containing JWT keys or connection strings

---

## WPF (LD.FormsX) — Mandatory UI Conventions

Apply these rules to **every** WPF view you read or touch. Apply in the same edit, no deferring.

### Rule 1 — Code-behind → ViewModel
Logic (guards, service calls, state updates) must live in the ViewModel using `[RelayCommand]` / `[ObservableProperty]`.  
Permitted in code-behind: `InitializeComponent()`, `DragMove()`, `PasswordChanged` bridge, `Loaded → vm.Method()`, dialog opening that needs `Window.GetWindow(this)`.  
Null guards (`if (x is null) return`) → always `CanExecute` on the command or computed property bound to `IsEnabled`.

### Rule 2 — Standardized input controls
- `<controls:LDInput>` instead of `TextBox` (including search fields).
- `<controls:LDSelector>` instead of `ComboBox`.
- Namespace: `xmlns:controls="clr-namespace:LD.FormsX.Controls"`.
- `LDInput.TextBoxElement` exposes the inner `TextBox` for `WpfGridFilter` compatibility.

### Rule 3 — No hex colors inline
All colors live in `src/LDForms/Resources/Styles/AppTheme.xaml` as named tokens.  
Never put `#RRGGBB` directly on a XAML property. Replace with `{StaticResource TokenName}`.  
Dashboard tile icon colors are semantic tokens (`IconClientes`, `IconAlmacen`, etc.) — not one generic accent.

### Rule 4 — Mandatory scan
Every time a WPF view is read or edited, apply Rules 1–3 in that same edit.

### Dashboard Refactor — Phase 1 (WPF)
- Use MVVM with CommunityToolkit.Mvvm for the dashboard refactor (Phase 1 only).
- Preserve the existing visible behavior and user interactions.
- Keep window chrome, resize, and titlebar logic in code-behind (allowed under Rule 1).
- Use a single metadata dictionary for modules (module id → metadata) and read tiles from it.
- Render dashboard tiles with ItemsControl + WrapPanel (avoid complex custom panels).
- Implement reactive search using a bound SearchText property and ICollectionView filtering.
- Do not implement any Phase 2 elements or advanced redesigns in this refactor.
- Apply these dashboard rules in the same edit that touches the dashboard view or viewmodel.

---

## MAUI — Mandatory UI Conventions

### Loading Modal — ALWAYS wrap HTTP calls with ShowBlocking/HideBlocking

**Rule**: In `LD.MobileApp`, every HTTP call made from a ViewModel **must** be wrapped with `_dialogService.ShowBlocking(...)` before and `_dialogService.HideBlocking()` in the `finally` block:

```csharp
try
{
    _dialogService.ShowBlocking("Loading", "Fetching data...");
    var result = await _someService.GetSomethingAsync();
}
catch (Exception ex)
{
    await _dialogService.ShowErrorAsync("Error", ex.Message);
}
finally
{
    _dialogService.HideBlocking();
}
```

Applies to: `[RelayCommand]` methods, `OnAppearing`/`OnNavigatedTo`, any method that calls `await _xxxService.XxxAsync(...)`. Does not apply to the login flow. When editing any ViewModel, add `ShowBlocking`/`HideBlocking` to any HTTP call that is missing it.

### IDialogService — NEVER use DisplayAlertAsync

`IDialogService` is registered as a Singleton in `MauiProgram.cs`. Inject it by constructor and use the correct method for every message type:

```csharp
private readonly IDialogService _dialogService;

await _dialogService.ShowErrorAsync("Error", ex.Message);
await _dialogService.ShowSuccessAsync("Guardado", "El registro fue guardado.");
await _dialogService.ShowInfoAsync("Atención", "Llene todos los campos.");
bool continuar = await _dialogService.ShowWarningAsync("¿Continuar?", "Se descartarán los cambios.");
_dialogService.ShowBlocking("Guardando", "Enviando registro...");   // before async POST
_dialogService.HideBlocking();                                       // always in finally
```

`DisplayPromptAsync` remains valid for text input capture only.

### PhotoGallery / PhotoThumb / ImagePreviewPopup — NEVER use CollectionView HorizontalList for photos

Custom controls live in `src/LD.MobileApp/Features/Controls/`.
Namespace: `xmlns:controls="clr-namespace:MauiAppLogin.Views.Controls"`

```xaml
<!-- Multiple photos (scrollable horizontal gallery) -->
<controls:PhotoGallery
    ItemsSource="{Binding Photos}"
    DeleteCommand="{Binding RemovePhotoCommand}"
    ViewCommand="{Binding ViewPhotoCommand}"
    MinimumHeightRequest="90"/>
```

```csharp
// ViewPhotoCommand in ViewModel
ViewPhotoCommand = new MvvmHelpers.Commands.Command<TPhotoItem>(item =>
{
    if (item?.Source is null) return;
    (Shell.Current.CurrentPage ?? Application.Current?.MainPage)
        ?.ShowPopup(new ImagePreviewPopup(item.Source));
});
```

- `PhotoGallery` bindable: `ItemsSource`, `DeleteCommand`, `ViewCommand`
- `PhotoThumb` bindable: `PhotoSource`, `DeleteCommand`, `DeleteCommandParameter`, `ViewCommand`, `ViewCommandParameter`
- Delete icon = trash can 🗑, never ✕ on photo thumbnails
- Tapping a photo always opens `ImagePreviewPopup`

### LabeledEntry / PasswordEntry — NEVER use raw Entry inside Border

Custom controls live in `src/LD.MobileApp/Features/Controls/`.
Namespace: `xmlns:controls="clr-namespace:MauiAppLogin.Views.Controls"`

```xaml
<!-- Correct — plain text -->
<controls:LabeledEntry
    LabelText="Nombre:"
    Text="{Binding Nombre}"
    Placeholder="Ej. Carlos Ramírez"/>

<!-- Correct — password -->
<controls:PasswordEntry
    LabelText="Contraseña:"
    Password="{Binding Password}"/>

<!-- Wrong -->
<Border><Entry Text="{Binding Nombre}"/></Border>
```

`LabeledEntry` bindable properties: `LabelText`, `Text` (TwoWay), `Placeholder`, `IconSource`.

### Mandatory Review Rule

When you read or edit **any** MAUI Page or ViewModel:
1. Replace every `<Entry>` inside a `<Border>` with `<controls:LabeledEntry>`.
2. Replace every `DisplayAlert` / `DisplayAlertAsync` / `DisplayActionSheet` with the appropriate `IDialogService` method.
3. Replace every `CollectionView HorizontalList` or manual photo thumbnail grid with `<controls:PhotoGallery>`.

There is a backlog of ~60 `DisplayAlertAsync` instances across `TaskSecurityViewModel`, `ChangeLocationViewModel`, `DamageReportDetailPage`, `ForkliftChecklistViewModel`, `PatioDetalleViewModel`, and others. Migrate them incrementally as each file is edited — not all at once.
