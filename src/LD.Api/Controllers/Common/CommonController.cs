using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers.Common;
[ApiController]
public abstract class CommonController : ControllerBase
{

    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}

