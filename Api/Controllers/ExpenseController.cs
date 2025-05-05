using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/expenses")]
public class ExpenseController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Personel")]
    public async Task<ApiResponse<ExpenseResponse>> Post([FromForm] ExpenseRequest model)
    {
        var result = await _mediator.Send(new CreateExpenseCommand(model));
        return result;
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{expenseId:guid}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid expenseId, [FromBody] UpdateExpenseStatusRequest model)
    {
        var result = await _mediator.Send(new UpdateExpenseStatusCommand(expenseId, model));
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> Get([FromQuery] ExpenseGetRequest model)
    {
        var result = await _mediator.Send(new GetExpensesQuery(model));
        return result;
    }
}
