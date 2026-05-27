using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Asn.Commands;
using LD.Application.Features.Asn.Queries;
using LD.Application.Features.AsnDetail.Commands;
using LD.Application.Features.AsnReceiptDetails.Commands;
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
        [AnyPermission(PermissionKeys.Asn_View, PermissionKeys.WarehouseStaff_Asn_View)]
        public async Task<IActionResult> GetAsnReceipt()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new AsnReceiptDetailQuery()));
        }

        [HttpGet("{asnReceiptId}")]
        [AnyPermission(PermissionKeys.Asn_View, PermissionKeys.WarehouseStaff_Asn_View)]
        public async Task<IActionResult> GetAsnReceiptById(int asnReceiptId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new AsnReceiptDetailByIdQuery { AsnReceiptDetailId = asnReceiptId }));

        [HttpGet("asnReceiptId/{asnDetailId}")]
        [AnyPermission(PermissionKeys.Asn_View, PermissionKeys.WarehouseStaff_Asn_View)]
        public async Task<IActionResult> GetAsnReceiptByAsnDetailId(int asnDetailId)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new AsnReceiptByAsnDetailIdQuery { AsnDetailId = asnDetailId }));
        }


        [HttpPost]
        [Permission(PermissionKeys.Asn_Create)]
        public async Task<IActionResult> CreateAsnReceipt([FromBody] CreateAsnReceiptDetailCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{asnReceiptId}")]
        [Permission(PermissionKeys.Asn_Update)]
        public async Task<IActionResult> UpdateAsn(int asnReceiptId, UpdateAsnReceiptDetailCommand command)
        {
            command.AsnReceiptDetailId = asnReceiptId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpDelete("{asnReceiptId}")]
        [Permission(PermissionKeys.Asn_Delete)]
        public async Task<IActionResult> DeleteAsnReceipt(int asnReceiptId)
        {
            var result = await Mediator.Send(new DeleteAsnReceiptDetailCommand { AsnReceiptDetailId = asnReceiptId });
            return ResultExtensions.ToActionResult(result);
        }
    }
}
