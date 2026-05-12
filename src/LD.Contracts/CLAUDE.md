# LD.Contracts — Project Guide for Claude Code

## Project Overview

`LD.Contracts` (net9.0) is the **shared contract library** consumed by both the API (`LD.Api`/`LD.Application`) and the client applications (`LD.MobileApp`, `LD.FormsX`, `LD.Client`). It contains only data-transfer types — no logic, no infrastructure, no services.

Every type in this project must be serialization-safe (no methods with side effects, no DI dependencies).

## Structure

```
LD.Contracts
├── Constants/
│   └── PermissionKeys.cs         ← Single source of truth for all permission key strings
├── DTOs/                          ← Read-only data models (often used directly as API responses)
│   ├── Auth/                      ← AuthorizationDto, ModuleAuthorizationDto, PermissionAuthorizationDto
│   ├── Asn/ AsnDetail/ AsnReceipt/
│   ├── Category/ Family/ Dimensioner/ Product/ Units/ Currency/ InventaryStatus/ Vehicle/
│   ├── Client/
│   ├── Equipment/ EquipmentQuestion/ EquipmentSupplier/ EquipmentType/
│   ├── InventoryMovement/
│   ├── Location/ Warehouse/ Project/
│   ├── Security/                  ← CortinaDto, SecurityRegistrationDto, SecurityTaskDto
│   ├── StandardLabel/
│   ├── User/                      ← UserDto, RoleDto, PermissionDto, GetUsersDtos
│   ├── DropDownDto.cs             ← Generic Key/Value pair for dropdowns/lookups
│   ├── LookupsDto.cs
│   └── UserProjectClientDto.cs
├── Requests/                      ← Input models sent by clients to the API
│   ├── LoginRequest.cs
│   ├── CategoryRequest.cs / FamilyRequest.cs / ...
│   ├── AsnRequest.cs / AsnDetailRequest.cs / AsnReceiptRequest.cs / ...
│   ├── InventoryMovementRequest.cs / ScanConfigurationRequest.cs / ...
│   └── ...
├── Responses/
│   ├── ApiResponseDto.cs          ← Generic response wrapper used by LD.Client consumers
│   ├── LoginResponse.cs
│   └── GetMeReponse.cs
├── Enums/
│   ├── DialogMessageEnum.cs
│   ├── DocumentType_e.cs / MovementType_e.cs / RegistroEstado_e.cs
│   ├── ScanSaveType_e.cs / ScanType_e.cs / SystemField_e.cs
│   ├── StorageType_e.cs
│   └── Module_e.cs
├── AvailableInventory/            ← AvailableInventoryDto + change request types
├── Checklist/                     ← ChecklistDetailDto, SubmitChecklistRequest, SubmitChecklistResponse, etc.
├── Equipment/                     ← EquipmentImageUploadDto
├── InventarioCiclico/             ← CyclicInventoryDto, CyclicInventoryDetailDto
└── ModuleKeys.cs (in Constants/)  ← Module ID constants
```

## Key Types

### `ApiResponseDto<T>` (Responses/)

Used by `LD.Client` services as the return type of every API call:

```csharp
public class ApiResponseDto<T>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public T? Data { get; init; }
    public int Code { get; init; }
    public List<string>? Errors { get; init; }
    public string? ErrorMessage => /* joins Errors */;
    public string Message { get; init; } = string.Empty;
}
```

This mirrors `Result<T>` from `LD.Application` — the API serializes `Result<T>` and the client deserializes it as `ApiResponseDto<T>`.

### `PermissionKeys` (Constants/)

**Canonical source for ALL permission strings.** Format: `"<domain>.<action>"` (e.g., `"categories.read"`, `"warehouse-staff.asn.execute"`).

**Never invent permission strings in controllers or clients.** Always use `PermissionKeys.X_Y`.

```csharp
public static class PermissionKeys
{
    public const string Category_View   = "categories.read";
    public const string Category_Create = "categories.create";
    // ...
}
```

### `DropDownDto` (DTOs/)

Generic lookup item:
```csharp
public class DropDownDto { public int Key { get; set; } public string Value { get; set; } }
```

Used for all dropdown/combobox data in UIs.

### Request Types

Commands in `LD.Application` inherit from `Request` types:
```csharp
// In LD.Contracts
public class CategoryRequest { public string CategoryName { get; set; } ... }

// In LD.Application
public class CreateCategoryCommand : CategoryRequest, IRequest<Result<string>> { }
```

### Response vs DTO Decision Rule

- If the API returns the entity "as-is" → use the DTO from `DTOs/`
- If the response adds metadata (pagination, totals) → create a dedicated `Response` in `Responses/`
- Do NOT duplicate: if `FooDto` is sufficient as a response, do not create `FooResponse` that mirrors it

## Dependencies

No LD.* project references, no NuGet packages with logic. This project is intentionally minimal.

## Naming Conventions

| Type | Suffix | Example |
|---|---|---|
| Input model | `Request` | `CategoryRequest`, `AsnDetailRequest` |
| Output model (typed) | `Dto` | `CategoryDto`, `AsnDetailDto` |
| Generic response wrapper | `ApiResponseDto<T>` | — |
| Enum file | `_e` suffix | `ScanType_e.cs`, `RegistroEstado_e.cs` |

## Common Tasks

1. **Add a new Request**: Create `Requests/<NewFeatureRequest>.cs` with properties matching what the client sends.
2. **Add a new DTO**: Create `DTOs/<Feature>/<EntityDto>.cs` with flat, serialization-friendly properties.
3. **Add a new permission key**: Add a `public const string X_Y = "domain.action"` to `PermissionKeys.cs`. Then seed it in the DB via a migration.
4. **Add a new enum**: Create `Enums/<Name>_e.cs`.

## Things Claude Must NOT Do

- Do not add methods with business logic to any type here
- Do not reference `LD.Domain` — contracts must not depend on domain entities
- Do not create a `FooResponse` that is identical to `FooDto` — reuse the DTO
- Do not hardcode permission strings in controllers; always use `PermissionKeys`
- Do not add EF attributes here — this is not a persistence layer
