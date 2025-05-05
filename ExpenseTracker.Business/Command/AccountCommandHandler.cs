namespace ExpenseTracker.Business.Command;

using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence.Domain;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;



public class AccountCommandHandler :
    IRequestHandler<CreateAccountCommand, ApiResponse<AccountResponse>>,
    IRequestHandler<UpdateAccountCommand, ApiResponse<AccountResponse>>,
    IRequestHandler<DeleteAccountCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public AccountCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<AccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = new Account
        {
            Id = Guid.NewGuid(),
            Name = request.Account.Name,
            IBAN = request.Account.IBAN,
            AccountNumber = request.Account.AccountNumber,
            CurrencyCode = request.Account.CurrencyCode,
            Balance = 0,
            UserId = _currentUser.Id,
            OpenDate = DateTimeOffset.UtcNow,
            InsertedUser = _currentUser.Id,
            InsertedDate = DateTimeOffset.UtcNow,
            IsActive = true
        };

        await _unitOfWork.AccountRepository.AddAsync(entity);
        await _unitOfWork.CompleteAsync();

        var response = new AccountResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            IBAN = entity.IBAN,
            AccountNumber = (int)entity.AccountNumber,
            CurrencyCode = entity.CurrencyCode,
            Balance = entity.Balance,
            OpenDate = entity.OpenDate.DateTime
        };

        return new ApiResponse<AccountResponse>(response);
    }

    public async Task<ApiResponse<AccountResponse>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.AccountRepository.GetByIdAsync(request.Id);
        if (entity == null)
            return new ApiResponse<AccountResponse>("Account not found");

        entity.Name = request.Account.Name;
        entity.IBAN = request.Account.IBAN;
        entity.AccountNumber = request.Account.AccountNumber;
        entity.CurrencyCode = request.Account.CurrencyCode;
        entity.UpdatedDate = DateTimeOffset.UtcNow;
        entity.UpdatedUser = _currentUser.Id;

        _unitOfWork.AccountRepository.Update(entity);
        await _unitOfWork.CompleteAsync();

        var response = new AccountResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            IBAN = entity.IBAN,
            AccountNumber = (int)entity.AccountNumber,
            CurrencyCode = entity.CurrencyCode,
            Balance = entity.Balance,
            OpenDate = entity.OpenDate.DateTime,
            CloseDate = entity.CloseDate?.DateTime
        };

        return new ApiResponse<AccountResponse>(response);
    }

    public async Task<ApiResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.AccountRepository.GetByIdAsync(request.Id);
        if (entity == null)
            return new ApiResponse("Account not found");

        _unitOfWork.AccountRepository.Delete(entity);
        await _unitOfWork.CompleteAsync();

        return new ApiResponse("Account deleted successfully");
    }
}
