using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.InventoryMovement.Commands;
using LD.Application.Features.InventoryMovement.Queries;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class InventoryMovementController : CommonController
    {
        [HttpGet]
        [Permission(PermissionKeys.Movement_View)]
        public async Task<IActionResult> GetInventoryMovement([FromQuery] int? standardId = null)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new InventoryMovementQuery
            {
                StandardId = standardId
            }));
        }

        [HttpGet("{movementId}")]
        [Permission(PermissionKeys.Movement_View)]
        public async Task<IActionResult> GetInventoryMovementById(int movementId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new InventoryMovementByIdQuery(movementId)));

        [HttpPost]
        [Permission(PermissionKeys.Movement_Create)]
        public async Task<IActionResult> CreateInventoryMovement([FromBody] CreateInventoryMovementCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{movementId}")]
        [Permission(PermissionKeys.Movement_Update)]
        public async Task<IActionResult> UpdateInventoryMovement(int movementId, UpdateInventoryMovementCommand command)
        {
            command.MovementId = movementId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }
    }
}
