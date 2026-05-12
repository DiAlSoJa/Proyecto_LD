# Copilot Instructions — LD.Domain

## Project Description
Innermost Clean Architecture layer (net9.0). Contains only pure C# entity classes and enums. Zero dependencies on any other project or NuGet package.

## Architectural Constraints

- No business logic in entities — properties only.
- No external package references allowed.
- No references to `LD.Application`, `LD.Infrastructure`, `LD.Contracts`, or any client project.
- EF entity configurations (Fluent API) go in `LdProyectDbContext.OnModelCreating()`, NOT here.
- Do NOT set `CreatedAt`, `LastModifiedAt`, or any audit field — the interceptor handles this automatically.

## Entity Template

```csharp
// Entities/Foo.cs
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

        [MaxLength(250)]
        public string? Description { get; set; }

        // Foreign keys
        public int? ClientId { get; set; }
        public int? ProjectId { get; set; }

        // Navigation properties
        public Client? Client { get; set; }
        public Project? Project { get; set; }

        // Collections
        public ICollection<FooDetail> Details { get; set; } = new List<FooDetail>();
    }
}
```

## AuditableEntity Fields (auto-filled — do not touch)

```
CreatedAt, CreatedByUserId, LastModifiedAt, LastModifiedByUserId,
DeletedAt, DeletedByUserId, IsActive
```

## File/Folder Structure

```
LD.Domain
├── Common/AuditableEntity.cs
├── Entities/<EntityName>.cs          ← One file per entity
└── Enums/<EnumName>_e.cs             ← Enum files use _e suffix
```

## Naming Conventions

- Entity class: `PascalCase` (e.g., `Category`, `AsnDetail`, `EquipmentType`)
- Primary key: `<ClassName>Id` (e.g., `CategoryId`)
- Foreign key: `<RelatedClass>Id` (e.g., `ClientId`, `ProjectId`)
- Navigation properties: same name as the related class
- Enum files: `<Name>_e.cs` (e.g., `MovementType_e.cs`)

## What to Avoid

- No methods on entities (no `Validate()`, `Calculate()`, computed logic with side effects)
- No NuGet packages
- No `IEntityTypeConfiguration<T>` classes
- No `using Microsoft.EntityFrameworkCore` — data annotations only for basic constraints
- No `static` fields or singleton state on entities
