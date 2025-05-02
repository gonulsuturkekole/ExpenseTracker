namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Persistence.Domain;
using ExpenseTracker.Schema;
using MediatR;

public record CreateExpenseCommand(ExpenseRequest Expense) : IRequest<ApiResponse<ExpenseResponse>>;
public record GetMyExpensesQuery(ExpenseStatus Status) : IRequest<ApiResponse<IEnumerable<ExpenseResponse>>>;
public record AdminGetAllExpensesQuery(ExpenseStatus Status) : IRequest<ApiResponse<IEnumerable<ExpenseResponse>>>;



