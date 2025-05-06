namespace ExpenseTracker.Business.Query;

using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class ExpenseQueryHandler
    : IRequestHandler<GetExpensesQuery, ApiResponse<IEnumerable<ExpenseResponse>>>
{
    private readonly ExpenseTrackerDbContext _dbContext;
    private readonly ICurrentUser _currentUser;

    public ExpenseQueryHandler(ExpenseTrackerDbContext dbContext, ICurrentUser currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var expensesQuery = _dbContext.Expenses.Include(x => x.Category).AsQueryable();
        if (request.Expense.Status.HasValue)
        {
            expensesQuery = expensesQuery.Where(x => x.Status == request.Expense.Status);
        }

        if (_currentUser.Role == UserRoles.Personel)
        {
            expensesQuery = expensesQuery.Where(x => x.UserId == _currentUser.Id);
        }

        var expenses = await expensesQuery.ToListAsync();
        return new ApiResponse<IEnumerable<ExpenseResponse>>(expenses.Select(x => new ExpenseResponse()
        {
            Id = x.Id,
            UserId = x.UserId,
            Amount = x.Amount,
            CategoryName = x.Category.Name,
            Description = x.Description,
            ExpenseDate = x.ExpenseDate,
            InsertedDate = x.InsertedDate,
            Location = x.Location,
            RejectReason = x.RejectReason,
            Status = x.Status,
            IsActive = x.IsActive
        }));
    }
}
