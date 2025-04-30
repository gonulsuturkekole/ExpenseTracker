using ExpenseTracker.Base;
using MediatR;


namespace ExpenseTracker.Business.Cqrs;

public record GetAllUsersQuery : IRequest<ApiResponse<List<UserResponse>>>;
public record GetUserByIdQuery(int Id) : IRequest<ApiResponse<UserResponse>>;
public record CreateUserCommand(UserRequest User) : IRequest<ApiResponse<UserResponse>>;
public record UpdateUserCommand(int Id, UserRequest User) : IRequest<ApiResponse>;
public record DeleteUserCommand(int Id) : IRequest<ApiResponse>;
