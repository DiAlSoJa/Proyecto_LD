using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Dimensioner.Comands;
using LD.Application.Features.Dimensioner.Queries;
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
    public class DimensionerController : CommonController
    {

        [HttpGet]
        [Permission(PermissionKeys.Dimensioner_View)]
        public async Task<IActionResult> GetDimensioner()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new DimensionerQuery()));


        }

        [HttpGet("{dimensionerId}")]
        [Permission(PermissionKeys.Dimensioner_View)]
        public async Task<IActionResult> GeUnitById(string dimensionerId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new DimensionerByIdQuery(dimensionerId)));


        [HttpPost]
        [Permission(PermissionKeys.Dimensioner_Create)]
        public async Task<IActionResult> CreateUnit([FromBody] CreateDimensionerCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{dimensionerId}")]
        [Permission(PermissionKeys.Dimensioner_Create)]
        public async Task<IActionResult> UpdateUnit(string dimensionerId, UpdateDimensionerCommand command)
        {
            command.DimensionerId = dimensionerId;
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
