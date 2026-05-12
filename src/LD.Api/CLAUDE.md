# LD.Api — Project Guide for Claude Code

## Project Overview

`LD.Api` is the ASP.NET Core Web API (net10.0) that serves as the single back-end for all three clients (MAUI, WPF, WinForms). It owns:
- Route definitions and HTTP verb mapping
- JWT authentication and dynamic permission-based authorization
- Request pipeline: logging, CORS, global exception handling
- Swagger/OpenAPI documentation (always enabled, not dev-only)
- Auto-migration of the database on startup

This project contains **no business logic**. All logic lives in `LD.Application`.

## Architecture

```
LD.Api
├── Controllers/          ← Route groups, one controller per entity/feature
│   ├── Common/           ← CommonController (base with Mediator property)
│   └── Mobile/           ← Mobile-specific controllers (e.g. SecurityController)
├── Authorization/        ← PermissionAttribute, PermissionHandler, PermissionRequirement, PermissionService
├── Middlewares/          ← GlobalExceptionMiddleware
├── Common/Results/       ← ResultExtensions (maps Result<T> to IActionResult)
└── Program.cs            ← DI wiring, JWT config, middleware pipeline, permission seeding
```

**Request flow:**
1. JWT middleware validates token
2. `PermissionHandler` checks the user's permission against DB-loaded policies
3. Controller action receives request, calls `Mediator.Send(command/query)`
4. `ResultExtensions.ToActionResult()` converts `Result<T>` to the correct HTTP status

## Key Classes

| Class | Purpose |
|---|---|
| `CommonController` | Abstract base; exposes `Mediator` (lazy-resolved) and `CurrentUserId` |
| `PermissionAttribute` | `[Permission("key")]` — sets the authorization policy to the given key |
| `PermissionHandler` | `AuthorizationHandler` that calls `IPermissionService.HasPermissionAsync` |
| `PermissionService` | Queries DB to check if the user has a specific permission |
| `GlobalExceptionMiddleware` | Catches unhandled exceptions; maps `UnauthorizedAccessException`, `KeyNotFoundException`, `ArgumentException` to proper HTTP codes |
| `ResultExtensions.ToActionResult<T>` | Switch on `Result<T>.Code` to return 200/400/401/403/404/409/422 |

## Dependencies

**Project references:**
- `LD.Application` — CQRS handlers, validators, behaviors
- `LD.Infrastructure` — EF context, repositories, identity, JWT service
- `LD.Contracts` — DTOs, Requests, Responses, PermissionKeys

**NuGet packages:**
- `MediatR 14.0.0`
- `Microsoft.AspNetCore.Authentication.JwtBearer 9.0.12`
- `Swashbuckle.AspNetCore 9.0.6`
- `Serilog.AspNetCore 10.0.0`
- `Microsoft.EntityFrameworkCore.Design 9.0.11` (design-time only)

## Controller Conventions

Every controller follows this pattern — **do not deviate**:

```csharp
[Authorize]
[Route("api/[controller]")]
public class FooController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.Foo_View)]
    public async Task<IActionResult> GetFoo()
        => ResultExtensions.ToActionResult(await Mediator.Send(new FooQuery()));

    [HttpPost]
    [Permission(PermissionKeys.Foo_Create)]
    public async Task<IActionResult> CreateFoo([FromBody] CreateFooCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    [HttpPut("{fooId}")]
    [Permission(PermissionKeys.Foo_Update)]
    public async Task<IActionResult> UpdateFoo(int fooId, UpdateFooCommand command)
    {
        command.FooId = fooId;
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }
}
```

Rules:
- Always inherit `CommonController`
- Always decorate with `[Authorize]` at the class level
- Always use `[Permission(PermissionKeys.X_Y)]` — never use `[Authorize(Policy = "...")]` directly
- The controller **only** calls `Mediator.Send(...)`. No business logic here.
- Use `ResultExtensions.ToActionResult(...)` for all responses

## How to Build and Run

```bash
# Run API (applies migrations automatically on startup)
dotnet run --project src/LD.Api

# The API listens on port 8050 by default
# Swagger UI available at: http://localhost:8050/swagger
```

## Configuration

`appsettings.json`:
- `ConnectionStrings:DefaultConnection` — SQL Server Express local connection
- `JwtSettings:Key`, `JwtSettings:Issuer` (`LdProyectAPI`), `JwtSettings:Audience` (`LdProyectClient`)

## Common Tasks in This Project

1. **Add a new controller**: Create `FooController.cs` in `Controllers/`, inherit `CommonController`, add `[Authorize]` and `[Route]`, add methods using the pattern above.
2. **Add a new permission**: First add the constant to `LD.Contracts/Constants/PermissionKeys.cs`, then seed it in the DB migration. Do NOT invent string literals here.
3. **Change error handling**: Only modify `GlobalExceptionMiddleware` for new exception types.

## Things Claude Must NOT Change Without Being Asked

- `Program.cs` — JWT pipeline, middleware order, Swagger config, permission loading loop
- The authorization pipeline order (`UseAuthentication` before `UseAuthorization`)
- CORS policy "AllowAll" — it is intentionally permissive
- The Swagger UI is always enabled (not gated by `IsDevelopment`)
- `GlobalExceptionMiddleware` registration position (must be last)
- Do not add logic to controllers — all logic goes in `LD.Application`
