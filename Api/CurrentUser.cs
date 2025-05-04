
using ExpenseTracker.Base;
using System.Security.Claims;

namespace ExpenseTracker.Api;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid Id => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserId"));
    public string UserName => _httpContextAccessor.HttpContext.User.FindFirstValue("UserName");
    public string FirstName => _httpContextAccessor.HttpContext.User.FindFirstValue("FirstName");
    public string LastName => _httpContextAccessor.HttpContext.User.FindFirstValue("LastName");
    public UserRoles Role => Enum.Parse<UserRoles>(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role));
}
