namespace ExpenseTracker.Business.Command;

using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Persistence.Domain;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class ExpenseCategoryCommandHandler
    : IRequestHandler<CreateExpenseCategoryCommand, ApiResponse<ExpenseCategoryResponse>>
{
    private readonly ExpenseTrackerDbContext _dbContext;
    private readonly ICurrentUser _currentUser;

    public ExpenseCategoryCommandHandler(ExpenseTrackerDbContext dbContext, ICurrentUser currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var isCategoryExists = await _dbContext.ExpenseCategories.AnyAsync(x => x.Name == request.Category.Name);
        if (isCategoryExists)
        {
            return new ApiResponse<ExpenseCategoryResponse>("category already exists");
        }

        var category = new ExpenseCategory()
        {
            Id = Guid.NewGuid(),
            Name = request.Category.Name,
            InsertedUser = _currentUser.Id,
            InsertedDate = DateTimeOffset.UtcNow,
            IsActive = true,
        };

        await _dbContext.ExpenseCategories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<ExpenseCategoryResponse>(new ExpenseCategoryResponse()
        {
            Id = category.Id,
            IsActive = true,
            InsertedDate = category.InsertedDate,
        });
    }
}