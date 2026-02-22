using LD.Api.Common.Results;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Comands;
using LD.Application.Features.Queries;
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
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetLocations([FromQuery] LocationQuery query)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(query));

        }
        [HttpGet("{locationId}")]
        public async Task<IActionResult> GetLocationById(int locationId)
             => ResultExtensions.ToActionResult(await _mediator.Send(new LocationByIdQuery(locationId)));

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(command));
        }

        [HttpPut("{locationId}")]
        public async Task<IActionResult> UpdateLocation(int locationId, UpdateLocationCommand command)
        {
            command.LocationId = locationId;
            var result = await _mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteContact(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
