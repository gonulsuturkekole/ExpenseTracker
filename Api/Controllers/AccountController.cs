using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<IEnumerable<AccountResponse>>> Get([FromQuery] AccountRequest model)
    {
        var result = await _mediator.Send(new GetAllAccountsQuery(model));
        return result;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<AccountResponse>> Create([FromBody] AccountRequest model)
    {
        var result = await _mediator.Send(new CreateAccountCommand(model));
        return result;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<AccountResponse>> Update(Guid id, [FromBody] AccountRequest model)
    {
        var result = await _mediator.Send(new UpdateAccountCommand(id, model));
        return result;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteAccountCommand(id));
        return result;
    }
}
    
