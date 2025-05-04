namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;

public record CreateExpenseCommand(ExpenseRequest Expense) : IRequest<ApiResponse<ExpenseResponse>>;
public record GetExpensesQuery(ExpenseGetRequest Expense) : IRequest<ApiResponse<IEnumerable<ExpenseResponse>>>;
public record UpdateExpenseStatusCommand(UpdateExpenseStatusRequest Expense) : IRequest<ApiResponse>;

