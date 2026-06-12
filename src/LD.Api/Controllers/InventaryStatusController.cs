using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.InventaryStatus.Comands;
using LD.Application.Features.Status.Queries;
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
    public class InventaryStatusController : CommonController
    {

        [HttpGet]
        [Permission(PermissionKeys.Status_View)]
        public async Task<IActionResult> GetStatus([FromQuery] int? clientId = null, [FromQuery] int? projectId = null)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new InventaryStatusQuery(clientId, projectId)));

        }

        [HttpGet("{statusId}")]
        [Permission(PermissionKeys.Status_View)]
        public async Task<IActionResult> GeStatusById(string statusId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new InventaryStatusByIdQuery(statusId)));


        [HttpPost]
        [Permission(PermissionKeys.Status_Create)]
        public async Task<IActionResult> CreateStatus([FromBody] CreateInventaryStatusCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{statusId}")]
        [Permission(PermissionKeys.Status_Update)]
        public async Task<IActionResult> UpdateStatus(string statusId, UpdateInventaryStatusCommand command)
        {
            command.InventoryStatusIdS = statusId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }
      
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteWarehouse(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
