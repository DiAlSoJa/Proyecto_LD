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

### 7. No Refresh Token
JWT tokens are short-lived. No refresh token flow is implemented. Do not implement it unless explicitly requested.

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

## Sensitive Files — Ask Before Changing

- `src/LD.Api/Program.cs`
- `src/LD.Infrastructure/Persistence/LdProyectDbContext.cs`
- `src/LD.Infrastructure/Persistence/Interceptors/AuditableEntitySaveChangesInterceptor.cs`
- `src/LD.Infrastructure/Services/Auth/JwtTokenService.cs`
- `src/LD.Api/Authorization/PermissionHandler.cs` / `PermissionService.cs`
- Any `appsettings.json` file containing JWT keys or connection strings
