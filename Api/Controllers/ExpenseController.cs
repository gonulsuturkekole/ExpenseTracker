using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence.Domain;
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

    [HttpPut("update-status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateExpenseStatusCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("my-expenses")]
    [Authorize(Roles = "Personel")]
    public async Task<IActionResult> GetMyExpenses([FromQuery] ExpenseStatus status)
    {
        var result = await _mediator.Send(new GetMyExpensesQuery(status));
        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllExpenses([FromQuery] ExpenseStatus status)
    {
        var response = await _mediator.Send(new AdminGetAllExpensesQuery(status));
        return Ok(response);
    }

}