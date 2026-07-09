using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.ReportQueries.Commands;
using LD.Application.Features.ReportQueries.Queries;
using LD.Contracts.Constants;
using LD.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ReportQueryController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.Query_View)]
    public async Task<IActionResult> GetReportQuerySummaries()
        => ResultExtensions.ToActionResult(await Mediator.Send(new ReportQuerySummaryQuery()));

    [HttpGet("manage")]
    [Permission(PermissionKeys.Query_Manage)]
    public async Task<IActionResult> GetReportQueries()
        => ResultExtensions.ToActionResult(await Mediator.Send(new ReportQueryQuery()));

    [HttpGet("{reportQueryId}")]
    [Permission(PermissionKeys.Query_Manage)]
    public async Task<IActionResult> GetReportQueryById(int reportQueryId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new ReportQueryByIdQuery(reportQueryId)));

    [HttpGet("{reportQueryId}/parameters")]
    [Permission(PermissionKeys.Query_Execute)]
    public async Task<IActionResult> GetReportQueryParameters(int reportQueryId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new GetReportQueryParametersQuery(reportQueryId)));

    [HttpPost]
    [Permission(PermissionKeys.Query_Manage)]
    public async Task<IActionResult> CreateReportQuery([FromBody] CreateReportQueryCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    [HttpPut("{reportQueryId}")]
    [Permission(PermissionKeys.Query_Manage)]
    public async Task<IActionResult> UpdateReportQuery(int reportQueryId, [FromBody] UpdateReportQueryCommand command)
    {
        command.ReportQueryId = reportQueryId;
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpDelete("{reportQueryId}")]
    [Permission(PermissionKeys.Query_Manage)]
    public async Task<IActionResult> DeleteReportQuery(int reportQueryId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new DeleteReportQueryCommand(reportQueryId)));

    [HttpPost("{reportQueryId}/execute")]
    [Permission(PermissionKeys.Query_Execute)]
    public async Task<IActionResult> ExecuteReportQuery(int reportQueryId, [FromBody] ReportQueryExecutionRequest request)
        => ResultExtensions.ToActionResult(await Mediator.Send(new ExecuteReportQueryCommand(reportQueryId, request.Parameters)));
}
