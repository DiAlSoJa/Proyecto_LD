using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Lookup.Queries;
using LD.Application.Features.Queries;
using LD.Contracts.Constants;
using LD.Domain.Entities;
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

        [HttpGet("warehouse/user/{userId}")]
        public async Task<IActionResult> GetWarehouseLookupByUser(string userId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetUserWarehouseLookupQuery(userId)));

        [HttpGet("location")]
        public async Task<IActionResult> GetLocationLookup()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetLocationLookupQuery()));

        [HttpGet("location/{warehouseId}")]
        public async Task<IActionResult> GetLocationByWarehouseLookup(int warehouseId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetLocationByWarehouseLookupQuery(warehouseId)));

        [HttpGet("client")]
        public async Task<IActionResult> GetClientLookup()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetClientLookupQuery()));

        [HttpGet("role")]
        public async Task<IActionResult> GetRoleLookup()
          => ResultExtensions.ToActionResult(await Mediator.Send(new GetRoleLookupQuery()));

        [HttpGet("systemfield")]
        public async Task<IActionResult> GetSystemFieldLookup()
        => ResultExtensions.ToActionResult(await Mediator.Send(new GetSystemFieldLookupQuery()));

        [HttpGet("project")]
        public async Task<IActionResult> GetProjectLookup()
          => ResultExtensions.ToActionResult(await Mediator.Send(new GetProjectLookupQuery()));

        [HttpGet("project/{clientId}")]        
        public async Task<IActionResult> GetProjectByClient(int clientId)
          => ResultExtensions.ToActionResult(await Mediator.Send(new GetProjecClienttLookupQuery(clientId)));

        [HttpGet("project-client/user/{userId}")]
        public async Task<IActionResult> GetProjectClientsByUserWarehouses(string userId)
          => ResultExtensions.ToActionResult(await Mediator.Send(new GetProjectClientsByUserWarehousesQuery(userId)));

        [HttpGet("category/{clientId}/{projectId}")]
        public async Task<IActionResult> GetCategoryByClientLookup(int clientId, int projectId)
         => ResultExtensions.ToActionResult(await Mediator.Send(new GetCategoryLookupQuery(clientId, projectId)));

        [HttpGet("family/{clientId}/{projectId}")]
        public async Task<IActionResult> GetFamilyByClientLookup(int clientId, int projectId)
         => ResultExtensions.ToActionResult(await Mediator.Send(new GetFamilyLookupQuery(clientId, projectId)));

        [HttpGet("unit")]
        public async Task<IActionResult> GetUnitLookup()
         => ResultExtensions.ToActionResult(await Mediator.Send(new GetUnitLookupQuery()));

        [HttpGet("dimensioner")]
        public async Task<IActionResult> GetDimensionerLookup()
         => ResultExtensions.ToActionResult(await Mediator.Send(new GetDimensionerLookupQuery()));

        [HttpGet("scantype")]
        public async Task<IActionResult> GetScanTypeLookup()
         => ResultExtensions.ToActionResult(await Mediator.Send(new GetScanTypeLookupQuery()));

        [HttpGet("scansavetype")]
        public async Task<IActionResult> GetScanSaveTypeLookup()
         => ResultExtensions.ToActionResult(await Mediator.Send(new GetScanSaveTypeLookupQuery()));
    }
}
