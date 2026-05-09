using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Asn.Commands;
using LD.Application.Features.Asn.Queries;
using LD.Application.Features.Family.Queries;
using LD.Contracts.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AsnController : CommonController
    {
        [HttpGet]
        [Permission(PermissionKeys.Asn_View)]
        public async Task<IActionResult> GetAsn()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new AsnQuery()));
        }

    
        [HttpGet("{asnId}")]
        [Permission(PermissionKeys.Asn_View)]
        public async Task<IActionResult> GetAsnById(int asnId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new AsnByIdQuery(asnId)));




        [HttpGet("{clientId}/{projectId}")]
        [Permission(PermissionKeys.Asn_View)]
        public async Task<IActionResult> GetAsnByClient(int clientId, int projectId)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new AsnByClientIdQuery { ClientId = clientId, ProjectId=projectId }));
        }


        [HttpPost]
        [Permission(PermissionKeys.Asn_Create)]
        public async Task<IActionResult> CreateAsn([FromBody] CreateAsnCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{asnId}")]
        [Permission(PermissionKeys.Asn_Update)]
        public async Task<IActionResult> UpdateAsn(int asnId, UpdateAsnCommand command)
        {
            command.AsnId = asnId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpPost("{asnId}/confirm")]
        [Permission(PermissionKeys.Asn_Update)]
        public async Task<IActionResult> ConfirmAsn(int asnId)
        {
            var command = new ConfirmAsnCommand
            {
                AsnId = asnId,
                UserId = CurrentUserId
            };

            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPost("{asnId}/cancel")]
        [Permission(PermissionKeys.Asn_Update)]
        public async Task<IActionResult> CancelAsn(int asnId)
        {
            var command = new CancelAsnCommand
            {
                AsnId = asnId,
                UserId = CurrentUserId
            };

            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }






    }
}
