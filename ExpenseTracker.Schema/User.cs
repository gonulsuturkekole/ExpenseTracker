
using ExpenseTracker.Base;

public class UserRequest : BaseRequest
{
    public string UserName { get; set; }
    public UserRoles Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}

public class UserResponse : BaseResponse
{
    public string UserName { get; set; }
    public UserRoles Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
}