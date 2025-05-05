using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;

namespace ExpenseTracker.Business.Query;

public class GetAllAccountsQueryHandler
    : IRequestHandler<GetAllAccountsQuery, ApiResponse<IEnumerable<AccountResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAccountsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<AccountResponse>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.AccountRepository.GetAllAsync();

        var response = accounts.Select(account => new AccountResponse
        {
            Id = account.Id,
            Name = account.Name,
            AccountNumber = (int)account.AccountNumber,
            IBAN = account.IBAN,
            Balance = account.Balance,
            CurrencyCode = account.CurrencyCode,
            OpenDate = account.OpenDate.DateTime,
            CloseDate = account.CloseDate?.DateTime ?? null
        });

        return new ApiResponse<IEnumerable<AccountResponse>>(response);
    }
}
