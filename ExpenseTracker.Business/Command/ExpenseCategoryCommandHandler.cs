namespace ExpenseTracker.Business.Command;
using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence.Domain;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;

public class ExpenseCategoryCommandHandler :
    IRequestHandler<CreateExpenseCategoryCommand, ApiResponse<ExpenseCategoryResponse>>,
    IRequestHandler<UpdateExpenseCategoryCommand, ApiResponse<ExpenseCategoryResponse>>,
    IRequestHandler<DeleteExpenseCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public ExpenseCategoryCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var isCategoryExists = await _unitOfWork.ExpenseCategoryRepository.AnyAsync(x => x.Name == request.Category.Name);
        if (isCategoryExists)
            return new ApiResponse<ExpenseCategoryResponse>("category already exists");

        var category = new ExpenseCategory()
        {
            Id = Guid.NewGuid(),
            Name = request.Category.Name,
            InsertedUser = _currentUser.Id,
            InsertedDate = DateTimeOffset.UtcNow,
            IsActive = true,
        };

        await _unitOfWork.ExpenseCategoryRepository.AddAsync(category);
        await _unitOfWork.CompleteAsync();

        return new ApiResponse<ExpenseCategoryResponse>(new ExpenseCategoryResponse()
        {
            Id = category.Id,
            IsActive = true,
            InsertedDate = category.InsertedDate,
        });
    }

    public async Task<ApiResponse> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.ExpenseCategoryRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (category == null)
        {
            return new ApiResponse("Category not found");
        }

        _unitOfWork.ExpenseCategoryRepository.Delete(category);
        await _unitOfWork.CompleteAsync();

        return new ApiResponse("Category deleted successfully");
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.ExpenseCategoryRepository.GetByIdAsync(request.Id);
        if (category == null)
            return new ApiResponse<ExpenseCategoryResponse>("Category not found");

        category.Name = request.Category.Name;
        category.UpdatedDate = DateTimeOffset.UtcNow;
        category.UpdatedUser = _currentUser.Id;

        _unitOfWork.ExpenseCategoryRepository.Update(category); 
        await _unitOfWork.CompleteAsync();

        return new ApiResponse<ExpenseCategoryResponse>(new ExpenseCategoryResponse()
        {
            Id = category.Id,
            IsActive = category.IsActive,
            InsertedDate = category.InsertedDate,
            UpdatedDate = category.UpdatedDate,
        });
    }

}
