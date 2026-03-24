using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Category.Comands;
using LD.Application.Features.Category.Queries;
using LD.Application.Features.Family.Comands;
using LD.Application.Features.Family.Queries;
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
    public class FamilyController : CommonController
    {

        [HttpGet]
        [Permission(PermissionKeys.Family_View)]
        public async Task<IActionResult> getFamily()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new FamilyQuery()));

        }

        [HttpGet("{familyId}")]
        [Permission(PermissionKeys.Family_View)]
        public async Task<IActionResult> GeFamilyById(int familyId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new FamilyByIdQuery(familyId)));


        [HttpPost]
        [Permission(PermissionKeys.Family_Create)]
        public async Task<IActionResult> CreateFamily([FromBody] CreateFamilyCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{familyId}")]
        [Permission(PermissionKeys.Family_Update)]
        public async Task<IActionResult> UpdateFamily(int familyId, UpdateFamilyCommand command)
        {
            command.FamilyId = familyId;
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
