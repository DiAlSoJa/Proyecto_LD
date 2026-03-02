using LD.Api.Common.Results;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LookupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLookups()
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(new GetLookupsQuery()));

        }
    }
}
