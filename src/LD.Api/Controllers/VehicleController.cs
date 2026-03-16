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

        [HttpGet("{plates}")]
        [Permission(PermissionKeys.Vehicle_View)]
        public async Task<IActionResult> GeVehicleById(string plates)
            => ResultExtensions.ToActionResult(await Mediator.Send(new VehicleByIdQuery(plates)));


        [HttpPost]
        [Permission(PermissionKeys.Vehicle_Create)]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{plates}")]
        [Permission(PermissionKeys.Vehicle_Update)]
        public async Task<IActionResult> UpdateVehicle(string plates, UpdateVehicleCommand command)
        {
            command.Plates = plates;
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
