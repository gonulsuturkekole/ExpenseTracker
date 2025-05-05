using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/expense-categories")]
public class ExpenseCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<ExpenseCategoryResponse>> Post([FromBody] ExpenseCategoryRequest model)
    {
        var result = await _mediator.Send(new CreateExpenseCategoryCommand(model));
        return result;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<ExpenseCategoryResponse>> Put(Guid id, [FromBody] ExpenseCategoryRequest model)
    {
        var result = await _mediator.Send(new UpdateExpenseCategoryCommand(id, model));
        return result;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteExpenseCategoryCommand(id));
        return result;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<IEnumerable<ExpenseCategoryResponse>>> GetAll([FromQuery] ExpenseCategoryRequest model)
    {
        var result = await _mediator.Send(new GetAllExpenseCategoriesQuery(model));
        return result;

    }
}