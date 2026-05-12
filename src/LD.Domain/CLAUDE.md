# LD.Domain — Project Guide for Claude Code

## Project Overview

`LD.Domain` (net9.0) is the innermost layer of the Clean Architecture. It contains:
- Pure entity classes (POCOs) with no external dependencies
- The `AuditableEntity` base class
- Domain enums

This project has **zero dependencies** on other LD.* projects or external NuGet packages.

## Structure

```
LD.Domain
├── Common/
│   └── AuditableEntity.cs        ← Base class for all auditable entities
├── Entities/                      ← All entity classes
│   ├── Asn.cs
│   ├── AsnDetail.cs
│   ├── AsnReceiptDetail.cs
│   ├── AvailableInventory.cs
│   ├── Category.cs
│   ├── Checklist.cs / ChecklistAnswer.cs / ChecklistDefectMark.cs / ChecklistPhoto.cs
│   ├── Client.cs / ClientContact.cs / ClientFiscalData.cs
│   ├── Cortina.cs
│   ├── Currency.cs
│   ├── CyclicInventory.cs / CyclicInventoryDetail.cs
│   ├── Dimensioner.cs
│   ├── Driver.cs
│   ├── Equipment.cs / EquipmentQuestion.cs / EquipmentQuestionDet.cs / EquipmentSupplier.cs / EquipmentType.cs
│   ├── Family.cs
│   ├── InventaryStatus.cs
│   ├── InventoryMovement.cs
│   ├── Location.cs
│   ├── Module.cs
│   ├── Permission.cs / RolePermission.cs
│   ├── PickingZone.cs
│   ├── Printer.cs
│   ├── Product.cs
│   ├── Project.cs
│   ├── ScanConfiguration.cs / ScanSaveType.cs / ScanType.cs / SystemField.cs
│   ├── SecurityRegistration.cs / SecurityTask.cs
│   ├── StandarIdSequence.cs
│   ├── StandardLabel.cs
│   ├── StorageType.cs
│   ├── Units.cs
│   ├── UserWarehouse.cs
│   ├── Vehicle.cs
│   └── Warehouse.cs
└── Enums/
    ├── DocumentType_e.cs
    ├── MovementType_e.cs
    └── RegistroEstado.cs
```

## AuditableEntity Base Class

All entities that need audit trail inherit from `AuditableEntity`:

```csharp
public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedByUserId { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedByUserId { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
}
```

These fields are **filled automatically** by `AuditableEntitySaveChangesInterceptor` in Infrastructure. **Do not set them manually** in handlers or controllers.

## Entity Conventions

- Entities use data annotations for constraints (`[Key]`, `[Required]`, `[MaxLength]`)
- Navigation properties for foreign key relationships
- Primary keys are named `<EntityName>Id` (e.g., `CategoryId`, `AsnId`)
- Foreign keys are named `<RelatedEntity>Id` (e.g., `ClientId`, `ProjectId`)
- Collections are initialized: `public ICollection<X> Items { get; set; } = new List<X>();`

## Example Entity Pattern

```csharp
using LD.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities
{
    public class Foo : AuditableEntity
    {
        [Key]
        [Required]
        public int FooId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FooName { get; set; } = string.Empty;

        public int? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
```

## Key Domain Concepts

- **Client / Project**: Top-level tenant hierarchy. Most entities belong to a Client + Project pair.
- **Warehouse → Module → PickingZone → Location**: Physical hierarchy for inventory placement.
- **ASN (Advanced Shipping Notice)**: `Asn` → `AsnDetail` → `AsnReceiptDetail`. Tracks shipments and their receipt.
- **AvailableInventory**: Current stock record. Linked to `Product`, `Location`, `Client`, `Project`, `StandardLabel`.
- **ScanConfiguration**: Configurable scanning rules per `Project`, linking `SystemField`, `ScanType`, `ScanSaveType`.
- **Checklist**: Forklift inspection workflow — `Checklist` + `ChecklistAnswer` + `ChecklistDefectMark` + `ChecklistPhoto`.
- **SecurityRegistration / SecurityTask / Cortina**: Yard control (patio) tracking.
- **UserWarehouse**: Many-to-many linking `ApplicationUser` (in Infrastructure) to `Warehouse`.
- **Permission / RolePermission**: DB-driven permission system, not hardcoded in code.
- **StandardLabel / StandarIdSequence**: Physical label generation and sequential ID management.

## How EF Configuration Works

Entity configurations (fluent API, indexes, relationships) are defined directly in `LdProyectDbContext.OnModelCreating()` in `LD.Infrastructure`. Do **not** create `IEntityTypeConfiguration<T>` files.

## Building

This project compiles as a library; it has no entry point. It is built as part of the solution:

```bash
dotnet build src/LD.Domain/LD.Domain.csproj
```

## Common Tasks in This Project

1. **Add a new entity**: Create `Entities/<EntityName>.cs`, inherit `AuditableEntity` if audit trail is needed. Use data annotations. Add EF configuration in `LdProyectDbContext`.
2. **Add a new enum**: Create `Enums/<Name>_e.cs` (note the `_e` suffix convention observed in the codebase).

## Things Claude Must NOT Do

- Do not add NuGet package references to this project
- Do not reference `LD.Application`, `LD.Infrastructure`, `LD.Contracts`, or any client project
- Do not add business logic to entities (no methods, no computed behavior beyond simple properties)
- Do not set `CreatedAt`, `LastModifiedAt`, or audit fields — the interceptor handles this
- Do not create `IEntityTypeConfiguration` classes here; EF config goes in `LdProyectDbContext`
