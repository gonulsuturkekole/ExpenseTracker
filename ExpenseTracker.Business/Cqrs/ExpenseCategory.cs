
namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;

public record CreateExpenseCategoryCommand(ExpenseCategoryRequest Category) : IRequest<ApiResponse<ExpenseCategoryResponse>>;
public record GetAllExpenseCategoriesQuery(ExpenseCategoryRequest Category) : IRequest<ApiResponse<IEnumerable<ExpenseCategoryResponse>>>;
public record UpdateExpenseCategoryCommand(Guid Id, ExpenseCategoryRequest Category) : IRequest<ApiResponse<ExpenseCategoryResponse>>;
public record DeleteExpenseCategoryCommand(Guid Id) : IRequest<ApiResponse>;