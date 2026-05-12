# Copilot Instructions — LD.Client

## Project Description
Shared HTTP client library (net9.0) used by `LD.MobileApp` and `LD.FormsX`. Abstracts all HTTP calls to `LD.Api`. Contains `ApiService` (raw HTTP), feature-specific services, `ApiEndpoints` (URL registry), and static session state.

## Architectural Constraints

- Never create a second `HttpClient` or `HttpClientHandler`. Always use the existing `ApiService`.
- Every new feature service must be registered in `ConfigureServices.AddLDClient()`.
- Services only translate method calls to HTTP — no business logic.
- Do not reference `LD.Domain`, `LD.Application`, or `LD.Infrastructure`.
- All return types are `ApiResponseDto<T>` from `LD.Contracts`.

## Feature Service Template

```csharp
// Services/FooService.cs
using LD.Contracts.DTOs.Foo;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
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

        public async Task<ApiResponseDto<FooDto>> GetFooByIdAsync(int id)
            => await _api.GetAsync<ApiResponseDto<FooDto>>(
                _endpoints.Foo_GetById.Replace("{fooId}", id.ToString()));

        public async Task<ApiResponseDto<string>> CreateFooAsync(FooRequest request)
            => await _api.PostAsync<FooRequest, ApiResponseDto<string>>(_endpoints.Foo_Create, request);

        public async Task<ApiResponseDto<string>> UpdateFooAsync(int id, FooRequest request)
            => await _api.PutAsync<FooRequest, ApiResponseDto<string>>(
                _endpoints.Foo_Update.Replace("{fooId}", id.ToString()), request);
    }
}
```

## ApiEndpoints Template (add to ApiEndpoints.cs)

```csharp
// ======================
// FOO
// ======================
public string Foo_GetAll   => $"{_baseApi}/foo";
public string Foo_GetById  => $"{_baseApi}/foo/{{fooId}}";
public string Foo_Create   => $"{_baseApi}/foo";
public string Foo_Update   => $"{_baseApi}/foo/{{fooId}}";
public string Foo_Delete   => $"{_baseApi}/foo/{{fooId}}";
```

## Register in ConfigureServices.cs

```csharp
// Inside AddLDClient:
services.AddScoped<FooService>();
```

## File/Folder Structure

```
Configuration/
├── ApiSettings.cs          ← BaseUrl property
├── ApiEndpoints.cs         ← All URL strings
├── UserSession.cs          ← AccessToken, RefreshToken, LogOut()
└── UserData.cs             ← User identity, HasPermission(), HasModule(), SetUserData()
Services/
├── ApiService.cs           ← Base HTTP client (do not touch)
└── <Feature>Service.cs     ← One file per feature
ConfigureServices.cs        ← Register all services here
```

## Naming Conventions

- Service class: `<Feature>Service` (e.g., `CategoryService`, `AsnReceiptService`)
- Service methods: `Get<Feature>Async`, `Create<Feature>Async`, `Update<Feature>Async`
- Endpoint properties: `<Feature>_<Action>` (e.g., `Category_GetAll`, `Asn_Confirm`)

## What to Avoid

- Never add `if/else` business logic to service methods
- Never call the API URL string directly — always use `ApiEndpoints`
- Never forget to register a new service in `ConfigureServices.cs`
- Never add a second `HttpClient` — `ApiService` is the only one
- Never store non-static state in service classes (they are scoped)
