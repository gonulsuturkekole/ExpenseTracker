using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetMyExpensesQueryHandler
    : IRequestHandler<GetMyExpensesQuery, ApiResponse<IEnumerable<ExpenseResponse>>>
{
    private readonly ExpenseTrackerDbContext _dbContext;
    private readonly ICurrentUser _currentUser;

    public GetMyExpensesQueryHandler(ExpenseTrackerDbContext dbContext, ICurrentUser currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> Handle(GetMyExpensesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Expenses
            .Include(expense => expense.Category)
            .Where(expense => expense.UserId == _currentUser.Id);

        query = query.Where(expense => expense.Status == request.Status);

        var result = await query
            .Select(expense => new ExpenseResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Location = expense.Location,
                ExpenseDate = expense.ExpenseDate,
                CategoryName = expense.Category.Name,
                Status = expense.Status,
                RejectReason = expense.RejectReason
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<IEnumerable<ExpenseResponse>>(result);
    }

    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> Handle(AdminGetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Expenses
            .Include(expense => expense.Category)
            .Include(expense => expense.User)
            .Where(expense => expense.Status == request.Status);

        var result = await query
            .Select(expense => new ExpenseResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Location = expense.Location,
                ExpenseDate = expense.ExpenseDate,
                CategoryName = expense.Category.Name,
                Status = expense.Status,
                RejectReason = expense.RejectReason,
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<IEnumerable<ExpenseResponse>>(result);
    }

}
