# LD.Client — Project Guide for Claude Code

## Project Overview

`LD.Client` (net9.0) is the **shared HTTP client library** consumed by both `LD.MobileApp` and `LD.FormsX`. It abstracts all HTTP communication with `LD.Api` so that neither UI project needs to know about URLs or HTTP mechanics.

This project contains:
- `ApiService` — raw HTTP client (GET, POST, PUT, DELETE, multipart)
- Feature-specific service classes (`CategoryService`, `AsnService`, etc.)
- `ApiEndpoints` — centralized URL registry
- `ApiSettings` — base URL configuration
- `UserSession` / `UserData` — static session state shared between all features

## Structure

```
LD.Client
├── ConfigureServices.cs         ← AddLDClient(options => ...) extension method
├── Configuration/
│   ├── ApiSettings.cs           ← BaseUrl property
│   ├── ApiEndpoints.cs          ← All endpoint URL strings, derived from BaseUrl
│   ├── UserSession.cs           ← Static: AccessToken, RefreshToken, LogOut()
│   └── UserData.cs              ← Static: user identity, Authorization tree, HasPermission(), HasModule()
├── Services/
│   ├── ApiService.cs            ← Base HTTP client; attaches JWT; handles responses
│   └── <Feature>Service.cs      ← One service per feature area
└── Mappers/
    └── Class1.cs                ← Placeholder (not in use)
```

## ApiService

The single HTTP client for the entire application. Key behaviors:
- SSL validation is disabled (development convenience — `ServerCertificateCustomValidationCallback = true`)
- 20-second timeout
- JSON `Accept` header set by default
- Bearer token set via `SetBearerToken(token)` after login

Methods:
- `GetAsync<T>(endpoint)` — HTTP GET
- `PostAsync<TRequest, TResponse>(endpoint, body)` — HTTP POST with JSON body
- `PutAsync<TRequest, TResponse>(endpoint, body)` — HTTP PUT with JSON body
- `PostMultipartAsync<TResponse>(endpoint, content)` — multipart/form-data POST
- `DeleteAsync<T>(endpoint)` — HTTP DELETE
- `GetByteArrayAsync(endpoint)` — download raw bytes

On error, `HandleResponse<T>` attempts deserialization; if it fails and `T` is `ApiResponseDto<X>`, it creates a failure instance with the HTTP status code and body as message.

**Never create a second `HttpClient` or HTTP service.** All services must use the existing `ApiService`.

## Feature Services Pattern

Each feature service receives `ApiService` and `ApiEndpoints` via constructor injection:

```csharp
public class FooService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _endpoints;

    public FooService(ApiService api, ApiEndpoints endpoints)
    {
        _api = api;
        _endpoints = endpoints;
    }

    public async Task<ApiResponseDto<List<FooDto>>> GetFooAsync()
        => await _api.GetAsync<ApiResponseDto<List<FooDto>>>(_endpoints.Foo_GetAll);

    public async Task<ApiResponseDto<string>> CreateFooAsync(FooRequest request)
        => await _api.PostAsync<FooRequest, ApiResponseDto<string>>(_endpoints.Foo_Create, request);

    public async Task<ApiResponseDto<string>> UpdateFooAsync(int id, FooRequest request)
        => await _api.PutAsync<FooRequest, ApiResponseDto<string>>(
               _endpoints.Foo_Update.Replace("{fooId}", id.ToString()), request);
}
```

## ApiEndpoints

All URL strings are properties on `ApiEndpoints`. Pattern:
- Simple: `$"{_baseApi}/foo"`
- With path param: `$"{_baseApi}/foo/{{fooId}}"` — callers use `.Replace("{fooId}", id.ToString())`

When adding new endpoints, add the URL properties to `ApiEndpoints` grouped by feature with a comment block.

## UserSession and UserData

Both are **static classes** shared across all feature services and ViewModels. After login:
1. `UserSession.AccessToken` / `UserSession.RefreshToken` are set
2. `ApiService.SetBearerToken(UserSession.AccessToken)` is called
3. `UserData.SetUserData(getMeResponse)` populates user identity and the `Authorization` tree

`UserData.HasPermission(key)` checks the loaded authorization tree — used in ViewModels to show/hide UI elements.

`UserData.HasModule(moduleId)` checks if the user has access to a given module.

## DI Registration

**Use `AddLDClient` — never register services one by one:**

```csharp
// In App.xaml.cs or MauiProgram.cs
services.AddLDClient(options =>
{
    options.BaseUrl = configuration["ApiSettings:BaseUrl"] ?? "";
});
```

This registers `ApiEndpoints` (singleton), `ApiService` (scoped and singleton — note: both exist, prefer scoped), and every feature service as scoped.

**If you add a new feature service**, add it to `ConfigureServices.cs` inside `AddLDClient`.

## Registered Services (as of current codebase)

AuthService, ClientService, ModuleService, ProductService, LocationService, ProjectService, WarehouseService, UserService, RoleService, UnitService, EquipmentTypeService, EquipmentService, EquipmentSupplierService, EquipmentQuestionService, InventaryStatusService, CurrencyService, CategoryService, FamilyService, DimensionerService, VehicleService, SecurityService, PatioClientService, InventoryMovementService, AvailableInventoryService, CyclicInventoryService, StandardLabelService, LookupService, AsnService, AsnDetailService, AsnReceiptService, ChecklistService

## Dependencies

**Project references:** `LD.Contracts`

**NuGet:** `Microsoft.Extensions.DependencyInjection.Abstractions`, `Microsoft.Extensions.Options`

## Common Tasks

1. **Add a new service**: Create `Services/<Feature>Service.cs`, inject `ApiService` and `ApiEndpoints`, add methods, register in `ConfigureServices.cs`.
2. **Add new endpoints**: Add URL properties to `ApiEndpoints.cs` in the appropriate section.
3. **Store login state**: Use `UserSession` for tokens and `UserData` for user/authorization data.

## Things Claude Must NOT Do

- Do not create a second `HttpClient` — use `ApiService`
- Do not add a new service without registering it in `ConfigureServices.cs`
- Do not put business logic here — this layer only translates ViewModel calls to HTTP
- Do not reference `LD.Domain`, `LD.Application`, or `LD.Infrastructure`
- Do not store mutable state outside `UserSession` and `UserData`
