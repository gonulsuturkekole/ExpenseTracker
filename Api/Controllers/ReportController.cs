using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("counts")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<ReportCountResponse>> ExpenseCountReport([FromQuery] ReportCountRequest model)
    {
        var result = await _mediator.Send(new ReportCountQuery(model));   
        return result;
    }

    [HttpGet("counts/{personelId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<ReportCountResponse>> ExpenseCountReport([FromRoute] Guid personelId, [FromQuery] ReportCountRequest model)
    {
        var result = await _mediator.Send(new ReportCountQuery(new ReportCountRequest()));
        return result;
    }

    [HttpGet("breakdown/{personelId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<List<ReportBreakdownResponse>>> ExpenseBreakdown(
    [FromRoute] Guid personelId, [FromQuery] ReportBreakdownRequest model)
    {
        model.UserId = personelId;
        var result = await _mediator.Send(new ReportBreakdownQuery(model));
        return result;
    }
}
