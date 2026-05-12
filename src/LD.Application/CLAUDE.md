# LD.Application — Project Guide for Claude Code

## Project Overview

`LD.Application` (net9.0) is the business logic layer of the solution. It implements CQRS via MediatR and contains:
- Command and Query handlers (all business rules live here)
- FluentValidation validators
- AutoMapper profiles
- MediatR pipeline behaviors (validation, logging)
- Shared interfaces (repository contracts, auth contracts)
- The `Result<T>` type used throughout the solution

This project has **no infrastructure dependencies** (no EF Core, no HTTP clients, no identity). It only depends on `LD.Domain` and `LD.Contracts`.

## Architecture

```
LD.Application
├── AssemblyMarker.cs                    ← Marker type for assembly scanning
├── ConfigureServices.cs                 ← Registers validators and pipeline behaviors
├── Common/
│   ├── Behaviors/
│   │   ├── ValidationBehavior.cs        ← Runs FluentValidation before every handler
│   │   └── LoggingBehavior.cs           ← Logs request/response names via ILogger
│   ├── Interfaces/
│   │   ├── Auth/                        ← IAuthService, IJwtTokenService, IPermissionService, ICurrentUserService, IApplicationUserManager
│   │   ├── Repository/                  ← IRepository<T>, IArchiveRepository<T>, IExistsRepository, and feature-specific interfaces
│   │   └── ...                          ← IStandarIdService, IFileStorageService, etc.
│   ├── Models/                          ← JwtSettings, AuthResponse, RefreshToken
│   └── Results/
│       └── ResultT.cs                   ← Result<T> (IsSuccess, Data, Code, Message, Errors)
└── Features/
    └── <Feature>/
        ├── Commands/                    ← <Verb><Entity>Command.cs (Request + Handler in same file)
        ├── Queries/                     ← <Entity>Query.cs / <Entity>By<X>Query.cs
        ├── Validators/                  ← <Verb><Entity>Validator.cs
        └── Profiles/                    ← <Entity>Profile.cs (AutoMapper)
```

## Result<T>

All handlers return `Result<T>`. This is the unified response envelope:

```csharp
// Success
Result<string>.Success("Mensaje de éxito", data, code: 200)

// Failure
Result<string>.Failure("Mensaje de error", new List<string> { "detalle" }, code: 400)
```

Properties: `IsSuccess`, `IsFailure`, `Data`, `Code`, `Message`, `Errors`.

## CQRS Conventions

### Command (mutating operation)

```csharp
// File: Features/Foo/Commands/CreateFooCommand.cs
public class CreateFooCommand : FooRequest, IRequest<Result<string>> { }

public class CreateFooCommandHandler : IRequestHandler<CreateFooCommand, Result<string>>
{
    private readonly IRepository<Foo> _repo;
    private readonly IMapper _mapper;

    public CreateFooCommandHandler(IRepository<Foo> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateFooCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Foo>(request);
            var ok = await _repo.CreateAsync(entity);
            return ok
                ? Result<string>.Success("Creado con éxito", "")
                : Result<string>.Failure("Error al crear", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Error al crear", new List<string> { ex.Message });
        }
    }
}
```

### Query (read-only operation)

```csharp
// File: Features/Foo/Queries/FooQuery.cs
public class FooQuery : IRequest<Result<List<FooDto>>> { }

public class FooQueryHandler : IRequestHandler<FooQuery, Result<List<FooDto>>>
{
    private readonly IRepository<Foo> _repo;
    private readonly IMapper _mapper;

    // ...

    public async Task<Result<List<FooDto>>> Handle(FooQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetManyAsync();
        return Result<List<FooDto>>.Success(_mapper.Map<List<FooDto>>(items), "");
    }
}
```

### Validator

```csharp
// File: Features/Foo/Validators/CreateFooValidator.cs
public class CreateFooValidator : AbstractValidator<CreateFooCommand>
{
    public CreateFooValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre no debe estar vacío")
            .MaximumLength(50).WithMessage("El nombre no debe exceder 50 caracteres");
    }
}
```

### AutoMapper Profile

```csharp
// File: Features/Foo/Profiles/FooProfile.cs
public class FooProfile : Profile
{
    public FooProfile()
    {
        CreateMap<Foo, FooDto>();
        CreateMap<FooRequest, Foo>();
    }
}
```

## Pipeline Behaviors (order matters)

1. `LoggingBehavior<TRequest, TResponse>` — logs handler name before and after
2. `ValidationBehavior<TRequest, TResponse>` — runs all validators; if failures, returns `Result<string>.Failure(code: 400)` without calling the handler

Both behaviors are registered in `ConfigureServices.cs` as `IPipelineBehavior<,>`.

## Naming Conventions

| Type | Pattern | Example |
|---|---|---|
| Command | `<Verb><Entity>Command` | `CreateCategoryCommand` |
| Query (list) | `<Entity>Query` | `CategoryQuery` |
| Query (filter) | `<Entity>By<Criteria>Query` | `CategoryByIdQuery` |
| Validator | `<Verb><Entity>Validator` | `CreateCategoryValidator` |
| Profile | `<Entity>Profile` | `CategoryProfile` |
| Handler | `<CommandOrQuery>Handler` | `CreateCategoryCommandHandler` |

## Key Interfaces

| Interface | Purpose |
|---|---|
| `IRepository<T>` | `GetByIdAsync`, `GetManyAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync` |
| `IArchiveRepository<T>` | Soft-archive operations |
| `IAuthService` | Login (delegates to Identity) |
| `IJwtTokenService` | Generate/validate JWT |
| `IPermissionService` | Check user permission from DB |
| `IApplicationUserManager` | User/role management wrapping UserManager |
| `IUserContextService` | Current user ID from HTTP context |

## Dependencies

**Project references:** `LD.Domain`, `LD.Contracts`

**NuGet:** `MediatR 14.0.0`, `AutoMapper 12.0.1`, `FluentValidation 12.1.1`, `Microsoft.Extensions.DependencyInjection.Abstractions`

## Common Tasks in This Project

1. **Add a new feature handler**: Create the `Features/<Feature>/Commands/` or `Queries/` folder structure. Command/Handler go in the same file.
2. **Add a validator**: Create `Features/<Feature>/Validators/<Verb><Entity>Validator.cs`. It is auto-discovered by `AddValidatorsFromAssembly`.
3. **Add a mapper profile**: Create `Features/<Feature>/Profiles/<Entity>Profile.cs`. It is auto-discovered by `AddAutoMapper`.

## Things Claude Must NOT Do

- Do not put infrastructure code here (EF Core, HTTP clients, file I/O)
- Do not use `DbContext` directly in handlers — use `IRepository<T>` or feature-specific repository interfaces
- Do not create `IEntityTypeConfiguration` classes here
- Do not put multiple operations in one handler (one command/query = one operation)
- Do not skip the `try/catch` wrapper in handlers — return `Result<T>.Failure(...)` on exceptions
