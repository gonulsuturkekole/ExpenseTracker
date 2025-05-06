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
    public AccountRequest Account { get; set; }
}

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MinimumLength(3).MaximumLength(32);
        RuleFor(x => x.Role).IsInEnum();
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(24);

        RuleFor(x => x.Account).NotNull().WithMessage("At least one account is required.");
        RuleFor(x => x.Account).ChildRules(x =>
        {
            x.RuleFor(a => a.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
            x.RuleFor(a => a.CurrencyCode).NotEmpty().MinimumLength(2).MaximumLength(3);
            x.RuleFor(a => a.AccountNumber).GreaterThan(0);
            x.RuleFor(a => a.IBAN).NotEmpty().MinimumLength(26).MaximumLength(34).Matches(@"^TR\d{2}\d{4}\d{4}\d{4}\d{4}\d{4}\d{2}$").WithMessage("IBAN must be in the format TRxx xxxx xxxx xxxx xxxx xxxx xx");
        });
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