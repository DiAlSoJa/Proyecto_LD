using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Vehicle.Comands;
using LD.Application.Features.Vehicle.Queries;
using LD.Application.Features.Vehicule.Comands;
using LD.Application.Features.Warehouses.Queries;
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
    public class VehicleController : CommonController
    {

        [HttpGet]
        [Permission(PermissionKeys.Vehicle_View)]
        public async Task<IActionResult> GetVehicle()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new VehicleQuery()));

        }

        [HttpGet("{vehicleId}")]
        [Permission(PermissionKeys.Vehicle_View)]
        public async Task<IActionResult> GeVehicleById(int vehicleId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new VehicleByIdQuery(vehicleId)));


        [HttpPost]
        [Permission(PermissionKeys.Vehicle_Create)]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{vehicleId}")]
        [Permission(PermissionKeys.Vehicle_Update)]
        public async Task<IActionResult> UpdateVehicle(int vehicleId, UpdateVehicleCommand command)
        {
            command.VehicleId = vehicleId;
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
