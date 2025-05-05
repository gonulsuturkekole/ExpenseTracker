using ExpenseTracker.Persistence.Domain;

namespace ExpenseTracker.Persistence;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ExpenseTrackerDbContext _dbContext;
    private bool _disposed = false;

    public UnitOfWork(ExpenseTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IGenericRepository<User> UserRepository => new GenericRepository<User>(_dbContext);
    public IGenericRepository<Account> AccountsRepository => new GenericRepository<Account>(_dbContext);
    public IGenericRepository<AccountTransaction> AccountTransactionRepository => new GenericRepository<AccountTransaction>(_dbContext);
    public IGenericRepository<MoneyTransfer> MoneyTransferRepository => new GenericRepository<MoneyTransfer>(_dbContext);
    public IGenericRepository<Expense> ExpenseRepository => new GenericRepository<Expense>(_dbContext);
    public IGenericRepository<ExpenseDocument> ExpenseDocumentRepository => new GenericRepository<ExpenseDocument>(_dbContext);
    public IGenericRepository<ExpenseCategory> ExpenseCategoryRepository => new GenericRepository<ExpenseCategory>(_dbContext);

    public async Task CompleteAsync()
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "Error occurred while saving changes to the database.");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
