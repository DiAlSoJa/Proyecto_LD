using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LD.Api.Controllers.Common;
[ApiController]
public abstract class CommonController : ControllerBase
{

    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    protected string CurrentUserEmail =>
        User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
}
