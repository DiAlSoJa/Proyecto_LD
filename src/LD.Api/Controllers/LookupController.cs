using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LookupController : CommonController
    {
        [HttpGet]
        public async Task<IActionResult> GetLookups()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetLookupsQuery()));

        [HttpGet("warehouse")]
        public async Task<IActionResult> GetWarehouseLookup()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetWarehouseLookupQuery()));

        [HttpGet("location")]
        public async Task<IActionResult> GetLocationLookup()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetLocationLookupQuery()));

        [HttpGet("client")]
        public async Task<IActionResult> GetClientLookup()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetClientLookupQuery()));
    }
}
