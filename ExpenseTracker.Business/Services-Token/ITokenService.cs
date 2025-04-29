using ExpenseTracker.Persistence.Domain;

namespace ExpenseTracker.Business.Services;

public interface ITokenService
{
    public string GenerateToken(User user);
}