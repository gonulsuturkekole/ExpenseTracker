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

        var response = accounts.Select(x => new AccountResponse
        {
            Id = x.Id,
            Name = x.Name,
            AccountNumber = (int)x.AccountNumber
        });

        return new ApiResponse<IEnumerable<AccountResponse>>(response);
    }
}