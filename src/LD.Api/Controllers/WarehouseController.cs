using LD.Application.Features.Warehouses.Comands;
using LD.Application.Features.Warehouses.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
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
            return Ok(await _mediator.Send(query));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GeWarehouseById(int id)
            => Ok(await _mediator.Send(new WarehouseByIdQuery(id)));


        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateWarehouse()
        //{
        //    return Ok("update Contact");
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteWarehouse(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
