# Copilot Instructions — LD.Application

## Project Description
CQRS business logic layer (net9.0). Contains all handlers, validators, AutoMapper profiles, and pipeline behaviors. Uses MediatR, FluentValidation, and AutoMapper. Depends only on `LD.Domain` and `LD.Contracts`.

## Architectural Constraints

- All business logic lives here. No logic in controllers, repositories, or clients.
- One Command/Query = one operation. No handler handles two things.
- Handlers return `Result<T>` — never throw exceptions to callers.
- Commands inherit from `Request` types in `LD.Contracts/Requests/`.
- Validators are auto-discovered by assembly scan; no manual registration needed.
- AutoMapper profiles are auto-discovered; no manual registration needed.
- Do not reference `DbContext` or any EF type; use `IRepository<T>` or feature-specific interfaces.

## Command Template

```csharp
// Features/Foo/Commands/CreateFooCommand.cs
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

## Query Template

```csharp
// Features/Foo/Queries/FooQuery.cs
public class FooQuery : IRequest<Result<List<FooDto>>> { }

public class FooQueryHandler : IRequestHandler<FooQuery, Result<List<FooDto>>>
{
    private readonly IRepository<Foo> _repo;
    private readonly IMapper _mapper;

    public FooQueryHandler(IRepository<Foo> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<List<FooDto>>> Handle(FooQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetManyAsync();
        return Result<List<FooDto>>.Success(_mapper.Map<List<FooDto>>(items), "");
    }
}
```

## Validator Template

```csharp
// Features/Foo/Validators/CreateFooValidator.cs
public class CreateFooValidator : AbstractValidator<CreateFooCommand>
{
    public CreateFooValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre no puede estar vacío")
            .MaximumLength(100).WithMessage("El nombre no debe exceder 100 caracteres");
    }
}
```

## AutoMapper Profile Template

```csharp
// Features/Foo/Profiles/FooProfile.cs
public class FooProfile : Profile
{
    public FooProfile()
    {
        CreateMap<Foo, FooDto>();
        CreateMap<FooRequest, Foo>();
    }
}
```

## File/Folder Structure

```
Features/<Feature>/
├── Commands/<Verb><Entity>Command.cs   ← Request + Handler in the same file
├── Queries/<Entity>Query.cs            ← List query
├── Queries/<Entity>By<X>Query.cs       ← Filtered query
├── Validators/<Verb><Entity>Validator.cs
└── Profiles/<Entity>Profile.cs
```

## Naming Conventions

| Type | Pattern |
|---|---|
| Command | `Create/Update/Delete<Entity>Command` |
| Query (list) | `<Entity>Query` |
| Query (single) | `<Entity>By<Criteria>Query` |
| Validator | `Create/Update<Entity>Validator` |
| Profile | `<Entity>Profile` |
| Handler | `<Command/Query>Handler` (nested in same file) |

## What to Avoid

- Never add `using Microsoft.EntityFrameworkCore` or any EF namespace
- Never return bare exceptions — always return `Result<T>.Failure(...)`
- Never put two operations in a single handler
- Never skip validators for commands that accept user input
- Never use `[Inject]` or static service locators — use constructor injection
