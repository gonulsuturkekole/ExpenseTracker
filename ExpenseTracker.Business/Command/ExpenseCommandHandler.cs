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
using static System.Runtime.InteropServices.JavaScript.JSType;

public class ExpenseCommandHandler
    : IRequestHandler<CreateExpenseCommand, ApiResponse<ExpenseResponse>>
    , IRequestHandler<UpdateExpenseStatusCommand, ApiResponse>

{
    private readonly ExpenseTrackerDbContext _dbContext;
    private readonly ICurrentUser _currentUser;
    private readonly IFileService _fileService;

    public ExpenseCommandHandler(ExpenseTrackerDbContext dbContext, ICurrentUser currentUser, IFileService fileService)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
        _fileService = fileService;
    }

    public async Task<ApiResponse<ExpenseResponse>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense()
        {
            Id = Guid.NewGuid(),
            UserId = _currentUser.Id,
            Amount = request.Expense.Amount,
            CategoryId = request.Expense.CategoryId,
            ExpenseDate = request.Expense.ExpenseDate,
            Description = request.Expense.Description,
            Location = request.Expense.Location,
            Status = ExpenseStatus.Pending,
            InsertedUser = _currentUser.Id,
            InsertedDate = DateTimeOffset.UtcNow,
            IsActive = true,
            Documents = new List<ExpenseDocument>(),
        };

        string filePath;
        using (MemoryStream ms = new MemoryStream())
        {
            await request.Expense.File.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            filePath = await _fileService.UploadFileAsync(request.Expense.File.FileName, ms);
        }

        var expenseDocument = new ExpenseDocument()
        {
            Id = Guid.NewGuid(),
            ExpenseId = expense.Id,
            FileName = request.Expense.File.FileName,
            FilePath = filePath,
            InsertedDate = DateTimeOffset.UtcNow,
            InsertedUser = _currentUser.Id,
            IsActive = true,
        };

        await _dbContext.ExpenseDocuments.AddAsync(expenseDocument);
        await _dbContext.Expenses.AddAsync(expense);
        await _dbContext.SaveChangesAsync();

        return new ApiResponse<ExpenseResponse>(new ExpenseResponse()
        {
            Id = expense.Id,
        });
    }

    public async Task<ApiResponse> Handle(UpdateExpenseStatusCommand request, CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == request.Expense.ExpenseId);
        if (expense == null)
            return new ApiResponse("no expense found");

        expense.Status = request.Expense.Status;
        if (request.Expense.Status == ExpenseStatus.Rejected)
        {
            expense.RejectReason = request.Expense.RejectReason;
        } 

        await _dbContext.SaveChangesAsync();
        return new ApiResponse("expense updated");
    }
}