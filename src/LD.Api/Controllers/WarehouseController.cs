using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Warehouses.Comands;
using LD.Application.Features.Warehouses.Queries;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WarehouseController : CommonController
    {

        [HttpGet]
        public async Task<IActionResult> GetWarehouse()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new WarehouseQuery()));

        }

        [HttpGet("{warehouseId}")]
        public async Task<IActionResult> GeWarehouseById(int warehouseId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new WarehouseByIdQuery(warehouseId)));


        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{warehouseId}")]
        public async Task<IActionResult> UpdateWarehouse(int warehouseId, UpdateWarehouseCommand command)
        {
            command.WarehouseId = warehouseId;
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
