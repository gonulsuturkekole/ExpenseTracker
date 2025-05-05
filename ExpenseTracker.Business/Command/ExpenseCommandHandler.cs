namespace ExpenseTracker.Business.Command;

using ExpenseTracker.Base;
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

    public ExpenseCommandHandler(IUnitOfWork unitOfWork, IAccountService accountService, ICurrentUser currentUser, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _fileService = fileService;
        _accountService = accountService;
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
        });
    }

    public async Task<ApiResponse> Handle(UpdateExpenseStatusCommand request, CancellationToken cancellationToken)
    {
        var expense = await _unitOfWork.ExpenseRepository.FirstOrDefaultAsync(x => x.Id == request.ExpenseId);
        if (expense == null)
            return new ApiResponse("no expense found");

        expense.Status = request.Expense.Status;
        if (request.Expense.Status == ExpenseStatus.Approved)
        {
            var refNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            var feeAmount = expense.Amount * 0.02m;

            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(_currentUser.Id, "Account");

            var responseOutgoing  = await _accountService.CreateOutgoingAccountTransaction(currentUser.Account.AccountNumber, expense.Amount, feeAmount, "Masraf onaylandı", refNumber);
            if (!responseOutgoing.Success)
            {
                return new ApiResponse(responseOutgoing.Message);
            }

            var expenseOwnerUser = await _unitOfWork.UserRepository.GetByIdAsync(expense.UserId, "Account");
            var responseIncoming = await _accountService.CreateIncomingAccountTransaction(expenseOwnerUser.Account.AccountNumber, expense.Amount, "Masraf onaylandı", refNumber);
            if (!responseIncoming.Success)
            {
                return new ApiResponse(responseIncoming.Message);
            }

            await _unitOfWork.MoneyTransferRepository.AddAsync(new MoneyTransfer()
            {
                Id = Guid.NewGuid(),
                Amount = expense.Amount,
                FeeAmount = feeAmount,
                FromAccountId = currentUser.Account.Id,
                ToAccountId = expenseOwnerUser.Account.Id,
                ReferenceNumber = refNumber,
                TransactionDate = DateTimeOffset.UtcNow,
                InsertedDate = DateTimeOffset.UtcNow,
                InsertedUser = _currentUser.Id,
            });
        }
        else if (request.Expense.Status == ExpenseStatus.Rejected)
        {
            expense.RejectReason = request.Expense.RejectReason;
        }

        await _unitOfWork.CompleteAsync();
        return new ApiResponse("expense updated");
    }
}