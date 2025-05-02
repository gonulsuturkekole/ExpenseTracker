namespace ExpenseTracker.Schema;

using ExpenseTracker.Base;
using FluentValidation;

public class UserRequest : BaseRequest
{
    public string UserName { get; set; }
    public UserRoles Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MinimumLength(6).MaximumLength(32);
        RuleFor(x => x.Role).IsInEnum();
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(24);
    }
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