using ExpenseTracker.Base.Domain;
using System.Linq.Expressions;

namespace ExpenseTracker.Persistence;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task SaveChangesAsync();
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetByIdAsync(Guid id, params string[] includes);
    Task<List<TEntity>> GetAllAsync(params string[] includes);
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
    Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate, params string[] includes);
    Task<TEntity> AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task DeleteByIdAsync(Guid id);
}
