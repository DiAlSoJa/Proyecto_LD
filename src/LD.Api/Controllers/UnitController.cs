using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Units.Comands;
using LD.Application.Features.Units.Queries;
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
    public class UnitController : CommonController
    {

        [HttpGet]
        [Permission(PermissionKeys.Unit_View)]
        public async Task<IActionResult> GetUnit()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new UnitQuery()));

        }

        [HttpGet("{unitId}")]
        [Permission(PermissionKeys.Unit_View)]
        public async Task<IActionResult> GeUnitById(int unitId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new UnitByIdQuery(unitId)));


        [HttpPost]
        [Permission(PermissionKeys.Unit_Create)]
        public async Task<IActionResult> CreateUnit([FromBody] CreateUnitCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{unitId}")]
        [Permission(PermissionKeys.Unit_Create)]
        public async Task<IActionResult> UpdateUnit(int unitId, UpdateUnitCommand command)
        {
            command.UnitId = unitId;
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
