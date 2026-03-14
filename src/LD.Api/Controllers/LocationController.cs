using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Comands;
using LD.Application.Features.Queries;
using LD.Contracts.Constants;
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
    public class LocationController : CommonController
    {

        [HttpGet]
        [Permission(PermissionKeys.Location_View)]
        public async Task<IActionResult> GetLocations()
            => ResultExtensions.ToActionResult(await Mediator.Send(new LocationQuery()));

        
        [HttpGet("{locationId}")]
        [Permission(PermissionKeys.Location_View)]
        public async Task<IActionResult> GetLocationById(int locationId)
             => ResultExtensions.ToActionResult(await Mediator.Send(new LocationByIdQuery(locationId)));

        [HttpPost]
        [Permission(PermissionKeys.Location_Create)]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command)
            => ResultExtensions.ToActionResult(await Mediator.Send(command));
        

        [HttpPut("{locationId}")]
        [Permission(PermissionKeys.Location_Update)]
        public async Task<IActionResult> UpdateLocation(int locationId, UpdateLocationCommand command)
        {
            command.LocationId = locationId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteContact(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
