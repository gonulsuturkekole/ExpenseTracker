
namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;

public record CreateUserCommand(UserRequest User) : IRequest<ApiResponse<UserResponse>>;
public record GetUserByIdQuery(Guid Id) : IRequest<ApiResponse<UserResponse>>;
public record GetAllUsersQuery : IRequest<ApiResponse<IEnumerable<UserResponse>>>;
public record UpdateUserCommand(Guid Id, UserRequest User) : IRequest<ApiResponse<UserResponse>>;
public record DeleteUserCommand(Guid Id) : IRequest<ApiResponse>;
