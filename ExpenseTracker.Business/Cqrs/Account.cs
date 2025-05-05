using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Cqrs
{
    public record CreateAccountCommand(AccountRequest Account) : IRequest<ApiResponse<AccountResponse>>;
    public record GetAllAccountsQuery(AccountRequest Account) : IRequest<ApiResponse<IEnumerable<AccountResponse>>>;
    public record UpdateAccountCommand(Guid Id, AccountRequest Account) : IRequest<ApiResponse<AccountResponse>>;
    public record DeleteAccountCommand(Guid Id) : IRequest<ApiResponse>;
}
