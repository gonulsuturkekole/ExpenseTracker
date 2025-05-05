using ExpenseTracker.Base;

namespace ExpenseTracker.Business.Services;

public interface IAccountService
{
    Task<ApiResponse> CreateOutgoingAccountTransaction(long accountId, decimal amount, decimal feeAmount, string description, string refNumber);
    Task<ApiResponse> CreateIncomingAccountTransaction(long accountId, decimal amount, string description, string refNumber);
}
