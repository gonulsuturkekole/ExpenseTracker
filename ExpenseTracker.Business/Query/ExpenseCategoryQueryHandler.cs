using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;


public class ExpenseCategoryQueryHandler
    : IRequestHandler<GetAllExpenseCategoriesQuery, ApiResponse<IEnumerable<ExpenseCategoryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExpenseCategoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<ExpenseCategoryResponse>>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.ExpenseCategoryRepository.GetAllAsync();

        var response = categories.Select(x => new ExpenseCategoryResponse
        {
            Id = x.Id,
            Name = x.Name,
            IsActive = x.IsActive,
            InsertedDate = x.InsertedDate
        });

        return new ApiResponse<IEnumerable<ExpenseCategoryResponse>>(response);
    }
}
