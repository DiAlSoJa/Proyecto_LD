# Copilot Instructions — LD.Contracts

## Project Description
Shared contract library (net9.0) consumed by both the API side and all client applications. Contains only data-transfer types: DTOs, Requests, Responses, Enums, and Constants. No logic.

## Architectural Constraints

- No methods with side effects on any class.
- No references to `LD.Domain`, `LD.Application`, or `LD.Infrastructure`.
- No NuGet packages with logic (only serialization-safe primitives).
- Permission strings only exist in `PermissionKeys.cs` — nowhere else.
- Do NOT create a `FooResponse` if `FooDto` already carries all the needed data; reuse the DTO.

## Request Template

```csharp
// Requests/FooRequest.cs
namespace LD.Contracts.Requests
{
    public class FooRequest
    {
        public string FooName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
    }
}
```

## DTO Template

```csharp
// DTOs/Foo/FooDto.cs
namespace LD.Contracts.DTOs.Foo
{
    public class FooDto
    {
        public int FooId { get; set; }
        public string FooName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ClientName { get; set; }   // flattened from navigation
        public string? ProjectName { get; set; }
    }
}
```

## Adding a New Permission Key

```csharp
// PermissionKeys.cs — add in the appropriate section
public const string Foo_View   = "foos.read";
public const string Foo_Create = "foos.create";
public const string Foo_Update = "foos.update";
public const string Foo_Delete = "foos.delete";
```

After adding the constant, create a migration to seed it in the DB.

## File/Folder Structure

```
Constants/PermissionKeys.cs          ← All permission string constants
Constants/ModuleKeys.cs              ← Module ID constants
DTOs/<Feature>/<Entity>Dto.cs        ← Read-only data models
Requests/<Entity>Request.cs          ← Input from client to API
Responses/ApiResponseDto.cs          ← Generic response wrapper
Responses/LoginResponse.cs           ← Auth responses
Enums/<Name>_e.cs                    ← Enum definitions
AvailableInventory/                  ← Feature-specific folder example
Checklist/                           ← Feature-specific folder example
InventarioCiclico/                   ← Feature-specific folder example
```

## Naming Conventions

| Type | Suffix | Example |
|---|---|---|
| Input model | `Request` | `CategoryRequest` |
| Output model | `Dto` | `CategoryDto` |
| Generic wrapper | `ApiResponseDto<T>` | — |
| Enum file | `_e` suffix | `ScanType_e.cs` |
| Permission constant | `<Domain>_<Action>` | `Category_View` |

## What to Avoid

- Never add EF attributes (`[ForeignKey]`, `[InverseProperty]`) — these are for domain entities
- Never add `INotifyPropertyChanged` or observable patterns — this is pure data
- Never duplicate a DTO as a Response if the data is the same
- Never invent permission strings outside `PermissionKeys.cs`
- Never reference `LD.Domain` entities directly in DTOs
