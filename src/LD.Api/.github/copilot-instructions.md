# Copilot Instructions — LD.Api

## Project Description
ASP.NET Core Web API (net10.0) serving the LD WMS. Single backend for MAUI, WPF, and legacy WinForms clients. No business logic here — pure routing and HTTP concerns.

## Architectural Constraints

- Controllers must inherit `CommonController` (provides lazy `Mediator` property and `CurrentUserId`).
- Business logic lives in `LD.Application`. The controller only calls `Mediator.Send(...)`.
- Every endpoint that requires authorization must use `[Permission(PermissionKeys.X_Y)]`.
- Permission strings must come from `LD.Contracts.Constants.PermissionKeys`. Never invent strings.
- All responses must go through `ResultExtensions.ToActionResult(result)`.
- Do not modify `Program.cs`, JWT configuration, or the middleware pipeline without explicit instructions.

## Preferred Pattern for a New Controller

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

## File/Folder Structure

```
Controllers/
├── Common/CommonController.cs        ← Base class only
├── Mobile/                           ← Mobile-specific endpoints
└── <Feature>Controller.cs            ← One file per entity/feature
Authorization/
├── PermissionAttribute.cs
├── PermissionHandler.cs
├── PermissionRequirement.cs
└── PermissionService.cs
Middlewares/GlobalExceptionMiddleware.cs
Common/Results/ResultExtensions.cs
```

## Naming Conventions

- Controller: `<Entity>Controller` (e.g., `CategoryController`)
- Route: `[Route("api/[controller]")]` (uses class name, minus "Controller")
- Action methods: PascalCase verb + noun (e.g., `GetCategory`, `CreateCategory`, `UpdateCategory`)

## What to Avoid

- Do NOT put if/else business logic in controller actions
- Do NOT call repositories or services directly from controllers
- Do NOT use `[Authorize(Policy = "key")]` — use `[Permission("key")]`
- Do NOT skip `[Authorize]` on a new controller
- Do NOT create a new `IActionResult` helper — use `ResultExtensions.ToActionResult`
