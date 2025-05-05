namespace ExpenseTracker.Business.Command;

using EasyNetQ;
using ExpenseTracker.Base;
using ExpenseTracker.Base.Consumers;
using ExpenseTracker.Business.Consumers;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Business.Services;
using ExpenseTracker.Persistence;
using ExpenseTracker.Persistence.Domain;
using ExpenseTracker.Schema;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class ExpenseCommandHandler
    : IRequestHandler<CreateExpenseCommand, ApiResponse<ExpenseResponse>>
    , IRequestHandler<UpdateExpenseStatusCommand, ApiResponse>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IFileService _fileService;
    private readonly IAccountService _accountService;
    private readonly IBus _bus;

    public ExpenseCommandHandler(IUnitOfWork unitOfWork, IAccountService accountService, IBus bus, ICurrentUser currentUser, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _fileService = fileService;
        _accountService = accountService;
        _bus = bus;
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

        await _unitOfWork.ExpenseDocumentRepository.AddAsync(expenseDocument);
        await _unitOfWork.ExpenseRepository.AddAsync(expense);
        await _unitOfWork.CompleteAsync();

        return new ApiResponse<ExpenseResponse>(new ExpenseResponse()
        {
            Id = expense.Id,
            // TODO: tamamlanacak
        });
    }

    public async Task<ApiResponse> Handle(UpdateExpenseStatusCommand request, CancellationToken cancellationToken)
    {
        var expense = await _unitOfWork.ExpenseRepository.FirstOrDefaultAsync(x => x.Id == request.ExpenseId);
        if (expense == null)
            return new ApiResponse("no expense found");
        else if (expense.Status != ExpenseStatus.Pending)
            return new ApiResponse($"already {expense.Status}");

        expense.Status = request.Expense.Status;
        if (request.Expense.Status == ExpenseStatus.Approved)
        {
            var exchange = await _bus.Advanced.ExchangeDeclareAsync("expense_tracker", "topic", true, false);
            await _bus.Advanced.PublishAsync<ApprovedExpenseConsumerModel>(exchange, "approved_expense", false, new Message<ApprovedExpenseConsumerModel>(new ApprovedExpenseConsumerModel()
            {
                Amount = expense.Amount,
                ApprovedUserId = _currentUser.Id,
                ExpenseOwnerUserId = expense.UserId,
            }));
        }
        else if (request.Expense.Status == ExpenseStatus.Rejected)
        {
            expense.RejectReason = request.Expense.RejectReason;
        }

        _unitOfWork.ExpenseRepository.Update(expense);
        await _unitOfWork.CompleteAsync();
        return new ApiResponse("expense updated");
    }
}