
namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;

public record CreateAuthorizationTokenCommand(AuthorizationRequest Request) : IRequest<ApiResponse<AuthorizationResponse>>;
