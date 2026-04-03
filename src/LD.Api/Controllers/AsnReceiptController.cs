using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Asn.Commands;
using LD.Application.Features.Asn.Queries;
using LD.Application.Features.AsnDetail.Commands;
using LD.Application.Features.AsnReceiptDetails.Queries;
using LD.Contracts.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AsnReceiptController : CommonController
    {
        [HttpGet]
        [Permission(PermissionKeys.Asn_View)]
        public async Task<IActionResult> GetAsnReceipt()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new AsnReceiptDetailQuery()));
        }

        [HttpGet("{asnId}")]
        [Permission(PermissionKeys.Asn_View)]
        public async Task<IActionResult> GetAsnReceiptById(int asnReceiptId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new AsnReceiptDetailByIdQuery { AsnReceiptDetailId = asnReceiptId }));

        [HttpPost]
        [Permission(PermissionKeys.Asn_Create)]
        public async Task<IActionResult> CreateAsnReceipt([FromBody] CreateAsnReceiptDetailCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{asnId}")]
        [Permission(PermissionKeys.Asn_Update)]
        public async Task<IActionResult> UpdateAsn(int asnId, UpdateAsnReceiptDetailCommand command)
        {
            command.AsnDetailId = asnId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }
    }
}
