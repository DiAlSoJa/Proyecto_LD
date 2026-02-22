using LD.Api.Common.Results;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Items.Comands;
using LD.Application.Features.Items.Queries;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] ItemQuery query)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(query));

        }
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(int itemId)
             => ResultExtensions.ToActionResult(await _mediator.Send(new ItemByIdQuery(itemId)));

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(command));
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateLocation(int itemId, UpdateItemCommand command)
        {
            command.ItemId = itemId;
            var result = await _mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteClient(int id)
        //{
        //    return Ok("Delete client");
        //}


    }
}
