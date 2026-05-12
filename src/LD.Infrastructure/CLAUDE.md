# LD.Infrastructure — Project Guide for Claude Code

## Project Overview

`LD.Infrastructure` (net10.0) is the infrastructure layer. It provides concrete implementations of all interfaces defined in `LD.Application`. It owns:
- EF Core `DbContext` and all entity configurations
- ASP.NET Core Identity setup (`ApplicationUser`, `ApplicationRole`)
- JWT token generation
- Repository implementations
- The audit interceptor that auto-fills `CreatedAt`/`LastModifiedAt`
- Database seeders and migrations
- File storage service (local)
- Standard label service
- Background worker (`KeepAliveWorker`)
- Serilog logging configuration

## Structure

```
LD.Infrastructure
├── ConfigureServices.cs                     ← Extension methods: AddInfrastructureServices, AddInfrastructureRepositories
├── Identity/
│   ├── ApplicationUser.cs                   ← Extends IdentityUser; adds FullName, IsActive, UserRoles, UserWarehouses
│   ├── ApplicationRole.cs                   ← Extends IdentityRole
│   ├── ApplicationUserRole.cs               ← Extends IdentityUserRole<string>
│   └── ApplicationUserManager.cs            ← Wraps UserManager for user/role operations
├── Persistence/
│   ├── LdProyectDbContext.cs                ← Single DbContext; all EF configurations in OnModelCreating()
│   ├── Interceptors/
│   │   └── AuditableEntitySaveChangesInterceptor.cs  ← Auto-fills audit fields
│   ├── Seeders/
│   │   └── SeedUser.cs                     ← Seeds the default admin user
│   ├── AppTransaction.cs                    ← Transaction wrapper
│   └── TransactionManager.cs               ← ITransactionManager implementation
├── Repositories/
│   ├── Repository.cs                        ← Generic IRepository<T> base
│   ├── ArchiveRepository.cs                 ← IArchiveRepository<T>
│   ├── ExistRepository.cs                   ← IExistsRepository
│   └── <Feature>Repository.cs               ← Feature-specific repos (ClientRepository, AsnRepository, etc.)
├── Services/
│   ├── Auth/
│   │   ├── AuthService.cs                   ← IAuthService: Login via UserManager + SignInManager
│   │   ├── JwtTokenService.cs               ← IJwtTokenService: GenerateToken, GenerateRefreshToken
│   │   └── UserContextService.cs            ← IUserContextService: reads UserId from IHttpContextAccessor
│   ├── StandardLabel/
│   │   └── StandardLabelService.cs          ← IStandarIdService: generates standard IDs/labels
│   └── Storage/
│       └── LocalFileStorageService.cs       ← IFileStorageService: saves files to local disk
├── Mappers/
│   ├── RoleProfile.cs / UserProfile.cs / LookupProfile.cs  ← AutoMapper profiles for infrastructure DTOs
├── Workers/
│   └── KeepAliveWorker.cs                   ← Background service (keeps the DB connection alive)
├── Logging/
│   └── LoggingConfiguration.cs              ← Serilog sink configuration helpers
└── Migrations/                              ← EF Core migrations (do not edit manually)
```

## Key Classes

### Repository Pattern

`Repository<T>` provides: `GetByIdAsync(int)`, `GetByIdAsync(string)`, `GetManyAsync()`, `CreateAsync(T)`, `UpdateAsync(T)`, `DeleteAsync(T)`.

Feature-specific repositories extend this for complex queries (joins, filters, projections):

```csharp
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(LdProyectDbContext context) : base(context) { }

    // Add custom query methods here
    public async Task<List<Category>> GetByClientAsync(int clientId)
        => await _context.Categories.Where(c => c.ClientId == clientId).ToListAsync();
}
```

### AuditableEntitySaveChangesInterceptor

Intercepts `SavingChanges`/`SavingChangesAsync`. For `Added` entries: sets `CreatedAt`, `CreatedByUserId`, `LastModifiedAt`, `LastModifiedByUserId`. For `Modified` entries: updates `LastModifiedAt`, `LastModifiedByUserId`.

**Soft delete is currently disabled** (the deletion code is commented out). Real deletes are used.

### EF Configuration Convention

ALL entity configurations go directly in `LdProyectDbContext.OnModelCreating()`. Do not create separate `IEntityTypeConfiguration<T>` classes.

### Identity Setup

- `ApplicationUser` extends `IdentityUser` with `FullName`, `IsActive`, navigation to `UserRoles`, `UserWarehouses`
- `ApplicationRole` extends `IdentityRole`
- Configured with `AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<LdProyectDbContext>()`

## DI Registration

Two extension methods in `ConfigureServices.cs`:

1. `AddInfrastructureServices(IConfiguration)`:
   - Registers `LdProyectDbContext` with SQL Server + audit interceptor
   - Registers Identity
   - Registers `IJwtTokenService`, `IAuthService`, `IUserContextService`, `IFileStorageService`, `IStandarIdService`, `ITransactionManager`
   - Registers MediatR (scanning `LoginCommand.Assembly`)
   - Starts `KeepAliveWorker` hosted service

2. `AddInfrastructureRepositories(IConfiguration)`:
   - Registers generic `IRepository<>` → `Repository<>`
   - Registers every feature-specific repository interface → concrete class

**If you add a new repository**, register it in `AddInfrastructureRepositories`.

## Migrations

```bash
# Add migration (always specify both projects)
dotnet ef migrations add <FeatureName> --project src/LD.Infrastructure --startup-project src/LD.Api

# Apply migration
dotnet ef database update --project src/LD.Infrastructure --startup-project src/LD.Api
```

**Migration naming**: use the feature name, e.g. `AddChecklistFeature`, `UpdateAsnReceipt`. Never timestamps or numbers.

Migrations are applied automatically on API startup via `db.Database.Migrate()` in `Program.cs`.

## Dependencies

**Project references:** `LD.Domain`, `LD.Application`, `LD.Contracts`

**NuGet:** EF Core + SQL Server 9.0.11, Identity 2.3.9/9.0.11, SixLabors.ImageSharp 3.1.12, Serilog sinks (Console, File, MSSqlServer)

## Common Tasks

1. **Add a new repository**: Create `Repositories/<Feature>Repository.cs`, extend `Repository<T>`, define an interface in `LD.Application/Common/Interfaces/Repository/`, register in `AddInfrastructureRepositories`.
2. **Add entity config**: Open `LdProyectDbContext.cs`, add configuration in `OnModelCreating()`.
3. **Add a migration**: Use the `dotnet ef` command above with a descriptive feature name.

## Things Claude Must NOT Change Without Being Asked

- `LdProyectDbContext` — structural changes (removing DbSets, changing identity configuration) require confirmation
- `AuditableEntitySaveChangesInterceptor` — do not re-enable soft delete or change audit logic without discussion
- JWT key, issuer, audience configuration
- `KeepAliveWorker` logic
- Migration files — never edit them manually; always use `dotnet ef`
- Identity setup (AddIdentity call, token providers)
