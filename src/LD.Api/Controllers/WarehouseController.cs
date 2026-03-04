using LD.Api.Common.Results;
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
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehouseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetWarehouse([FromQuery] WarehouseQuery query)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(query));

        }

        [HttpGet("{warehouseId}")]
        public async Task<IActionResult> GeWarehouseById(int warehouseId)
            => ResultExtensions.ToActionResult(await _mediator.Send(new WarehouseByIdQuery(warehouseId)));


        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseCommand command)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(command));
        }

        [HttpPut("{warehouseId}")]
        public async Task<IActionResult> UpdateWarehouse(int warehouseId, UpdateWarehouseCommand command)
        {
            command.WarehouseId = warehouseId;
            var result = await _mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteWarehouse(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
