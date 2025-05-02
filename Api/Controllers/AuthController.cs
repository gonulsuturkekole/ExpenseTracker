namespace Api.Controllers;

using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("token")]
    public async Task<ApiResponse<AuthorizationResponse>> Post([FromBody] AuthorizationRequest request)
    {
        var operation = new CreateAuthorizationTokenCommand(request);
        var result = await _mediator.Send(operation);
        return result;
    }
}