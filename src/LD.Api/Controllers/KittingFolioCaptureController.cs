using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.KittingFolioCaptures.Commands;
using LD.Application.Features.KittingFolioCaptures.Queries;
using LD.Contracts.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class KittingFolioCaptureController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.KittingFolioCapture_Access)]
    public async Task<IActionResult> GetKittingFolioCaptures([FromQuery] int? clientId = null, [FromQuery] int? projectId = null)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new KittingFolioCaptureQuery
        {
            ClientId = clientId,
            ProjectId = projectId
        }));
    }

    [HttpGet("{captureId}")]
    [Permission(PermissionKeys.KittingFolioCapture_Access)]
    public async Task<IActionResult> GetKittingFolioCaptureById(int captureId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new KittingFolioCaptureByIdQuery
        {
            KittingFolioCaptureId = captureId
        }));

    [HttpPost("generate")]
    [Permission(PermissionKeys.KittingFolioCapture_Access)]
    public async Task<IActionResult> Generate([FromBody] GenerateKittingFolioCaptureCommand command)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpPost("preview")]
    [Permission(PermissionKeys.KittingFolioCapture_Access)]
    public async Task<IActionResult> Preview([FromBody] PreviewKittingFolioCaptureQuery query)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(query));
    }
}
