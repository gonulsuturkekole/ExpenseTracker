using ExpenseTracker.Persistence.Domain;
using Dapper;


namespace ExpenseTracker.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task CompleteAsync();
    
    Task<T> QuerySingleAsync<T>(string sql, DynamicParameters parameters);


    IGenericRepository<User> UserRepository { get; }
    IGenericRepository<Account> AccountRepository { get; }
    IGenericRepository<AccountTransaction> AccountTransactionRepository { get; }
    IGenericRepository<MoneyTransfer> MoneyTransferRepository { get; }
    IGenericRepository<Expense> ExpenseRepository { get; }
    IGenericRepository<ExpenseDocument> ExpenseDocumentRepository { get; }
    IGenericRepository<ExpenseCategory> ExpenseCategoryRepository { get; }
}
