using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Asn.Commands;
using LD.Application.Features.Asn.Queries;
using LD.Application.Features.AsnDetail.Commands;

using LD.Application.Features.AsnDetails.Queries;
using LD.Contracts.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AsnDetailController : CommonController
    {
        [HttpGet()]
        [Permission(PermissionKeys.Asn_View)]
        public async Task<IActionResult> GetAsnDetail()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new AsnDetailQuery()));
        }

        [HttpGet("{asnId}")]
        [Permission(PermissionKeys.Asn_View)]
        public async Task<IActionResult> GetAsnDetailById(int asnDetailId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new AsnDetailByIdQuery { AsnDetailId = asnDetailId }));
        
        [HttpGet("asn/{asnId}")]
        [Permission(PermissionKeys.Asn_View)]
        public async Task<IActionResult> GetAsnDetailByAsn(int asnId)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new AsnDetailByAsnIdQuery { AsnId = asnId }));
        }


        [HttpPost]
        [Permission(PermissionKeys.Asn_Create)]
        public async Task<IActionResult> CreateAsnDetail([FromBody] CreateAsnDetailCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{asnId}")]
        [Permission(PermissionKeys.Asn_Update)]
        public async Task<IActionResult> UpdateDeailAsn(int asnId, UpdateAsnDetailCommand command)
        {
            command.AsnId = asnId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }
    }
}
