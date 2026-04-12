# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Proyecto_LD** is a multi-client warehouse management system (WMS) for logistics operations. It consists of a single .NET backend API serving three client surfaces: a MAUI cross-platform mobile app, a WinForms desktop client, and a WPF desktop client.

## Build & Run Commands

```bash
# Restore all NuGet packages
dotnet restore

# Build entire solution
dotnet build LD.sln

# Run the backend API (starts on port 8050, applies DB migrations on startup)
dotnet run --project src/LD.Api

# Build mobile app
dotnet build src/LD.MobileApp/LD.MobileApp.csproj

# Build WinForms desktop
dotnet build src/LD.Forms/LD.Forms.csproj

# Build WPF desktop
dotnet build src/LDForms/LD.FormsX.csproj

# Apply EF Core migrations manually (normally auto-applied at API startup)
dotnet ef database update --project src/LD.Infrastructure --startup-project src/LD.Api

# Add a new migration
dotnet ef migrations add <MigrationName> --project src/LD.Infrastructure --startup-project src/LD.Api
```

There are no automated tests configured in this solution.

## Architecture

The solution follows Clean Architecture with these projects in dependency order:

```
Clients (MobileApp / Forms / FormsX)
    ↓
LD.Client          # HTTP API client library (21 services wrapping REST endpoints)
    ↓
LD.Api             # ASP.NET Core REST API (24 controllers)
    ↓
LD.Application     # CQRS handlers (MediatR), validators (FluentValidation), AutoMapper profiles
    ↓
LD.Infrastructure  # EF Core + SQL Server, Identity, JWT auth, Serilog
    ↓
LD.Domain          # Domain entities only — no external dependencies
    ↓
LD.Contracts       # Shared DTOs, enums, request/response models used across all layers
```

### Key Architecture Patterns

**CQRS via MediatR**: Application logic lives in `src/LD.Application/Features/<Domain>/Commands/` and `.../Queries/`. Each feature folder contains its handlers, validators, and AutoMapper profiles. Adding a new operation means: create a Request/Handler/Validator in `Application`, add a controller action in `Api`, and a client method in `LD.Client/Services/`.

**Repository Pattern**: `LD.Infrastructure/Repositories/` has a generic `Repository<T>` plus domain-specific overrides. The EF DbContext is `LdProyectDbContext` — all entity configurations are defined there (not in separate `IEntityTypeConfiguration` files).

**Permission System**: Permissions are stored in the database and loaded into a static collection at API startup (`Program.cs`). The `PermissionHandler` + `PermissionService` enforce granular access. Roles and their permissions are managed via `RoleController`.

**Audit Trail**: `AuditableEntitySaveChangesInterceptor` in Infrastructure automatically sets `CreatedAt`/`UpdatedAt` on all entities that inherit from the auditable base class.

**Mobile App (MAUI)**: Uses MVVM via `CommunityToolkit.Mvvm`. Each feature under `LD.MobileApp/Features/<Feature>/` contains Pages (`.xaml`/`.xaml.cs`) and ViewModels. Navigation uses MAUI Shell (`AppShell.xaml`). Hardware integration includes ZXing for barcode scanning and `Plugin.Maui.OCR` for text recognition from camera.

**API Client (`LD.Client`)**: `ApiService` is the core HTTP client that attaches JWT tokens. Domain services (`ProductService`, `WarehouseService`, etc.) wrap specific endpoints. The base URL is configured in `appsettings.json` (`ApiSettings:BaseUrl` — default `http://192.168.0.112:8050/api` for local network).

## Key Configuration

**Database** (`src/LD.Api/appsettings.json`):
- Local: `Server=localhost\SQLEXPRESS;Database=Warehouse_System;Trusted_Connection=True;`
- Migrations auto-run on API startup via `app.MigrateDatabase()`

**JWT** (`appsettings.json → JwtSettings`): Symmetric key auth with issuer `LdProyectAPI` and audience `LdProyectClient`.

**Mobile API base URL**: Configured in `src/LD.MobileApp/MauiProgram.cs` and `appsettings.json` of the client projects. Update `ApiSettings:BaseUrl` to point to the running API server.

**Logging**: Serilog writes rolling JSON logs to `logs/` directory. Configured in `Program.cs` before host build.

## Domain Concepts

- **ASN (Advanced Shipping Notice)**: Inbound shipment declarations with detail lines (`AsnDetail`) and receipt records (`AsnReceiptDetail`).
- **Location hierarchy**: `Warehouse → Module → PickingZone → Location`
- **Scanning configurations**: `ScanConfiguration`, `ScanType`, `ScanSaveType`, `SystemField` — flexible barcode scanning workflows configurable per operation type.
- **UserWarehouse**: Many-to-many linking users to the warehouses they can operate in.
