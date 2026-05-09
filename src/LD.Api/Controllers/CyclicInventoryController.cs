using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.CyclicInventory.Commands;
using LD.Application.Features.CyclicInventory.Queries;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CyclicInventoryController : CommonController
    {
        [HttpGet]
        [Permission(PermissionKeys.CycleCount_View)]
        public async Task<IActionResult> GetCyclicInventories(
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta,
            [FromQuery] string? estatus)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new CyclicInventoryQuery
            {
                Desde = desde,
                Hasta = hasta,
                Estatus = estatus
            }));
        }

        [HttpGet("{cyclicInventoryId}")]
        [Permission(PermissionKeys.CycleCount_View)]
        public async Task<IActionResult> GetCyclicInventoryById(int cyclicInventoryId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new CyclicInventoryByIdQuery(cyclicInventoryId)));

        [HttpPost]
        [Permission(PermissionKeys.CycleCount_Create)]
        public async Task<IActionResult> CreateCyclicInventory([FromBody] CreateCyclicInventoryCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{cyclicInventoryId}")]
        [Permission(PermissionKeys.CycleCount_Update)]
        public async Task<IActionResult> UpdateCyclicInventory(int cyclicInventoryId, [FromBody] UpdateCyclicInventoryCommand command)
        {
            command.InventarioCiclicoId = cyclicInventoryId;
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }
    }
}
