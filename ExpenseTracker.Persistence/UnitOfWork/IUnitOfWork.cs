using ExpenseTracker.Persistence.Domain;

namespace ExpenseTracker.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task CompleteAsync();
    IGenericRepository<User> UserRepository { get; }
    IGenericRepository<Account> AccountsRepository { get; }
    IGenericRepository<AccountTransaction> AccountTransactionRepository { get; }
    IGenericRepository<MoneyTransfer> MoneyTransferRepository { get; }
    IGenericRepository<Expense> ExpenseRepository { get; }
    IGenericRepository<ExpenseDocument> ExpenseDocumentRepository { get; }
    IGenericRepository<ExpenseCategory> ExpenseCategoryRepository { get; }
}
