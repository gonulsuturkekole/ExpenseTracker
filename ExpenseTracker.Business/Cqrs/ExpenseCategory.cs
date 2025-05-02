
namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;

public record CreateExpenseCategoryCommand(ExpenseCategoryRequest Category) : IRequest<ApiResponse<ExpenseCategoryResponse>>;
