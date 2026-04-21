using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.EquipmentType.Comands;
using LD.Application.Features.EquipmentType.Queries;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EquipmentTypeController : CommonController
    {
        [HttpGet]
        [Permission(PermissionKeys.EquipmentType_View)]
        public async Task<IActionResult> GetEquipmentType()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new EquipmentTypeQuery()));
        }

        [HttpGet("{equipmentTypeId}")]
        [Permission(PermissionKeys.EquipmentType_View)]
        public async Task<IActionResult> GeEquipmentTypeById(int equipmentTypeId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new EquipmentTypeByIdQuery(equipmentTypeId)));

        [HttpPost]
        [Permission(PermissionKeys.EquipmentType_Create)]
        public async Task<IActionResult> CreateEquipmentType([FromBody] CreateEquipmentTypeCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{equipmentTypeId}")]
        [Permission(PermissionKeys.EquipmentType_Update)]
        public async Task<IActionResult> UpdateEquipmentType(int equipmentTypeId, UpdateEquipmentTypeCommand command)
        {
            command.EquipmentTypeId = equipmentTypeId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }
    }
}
