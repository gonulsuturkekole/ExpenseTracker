
using ExpenseTracker.Base;
using FluentValidation;

namespace ExpenseTracker.Schema;

public class AuthorizationRequest : BaseRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class AuthorizationRequstValidator : AbstractValidator<AuthorizationRequest>
{
    public AuthorizationRequstValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class AuthorizationResponse
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public DateTime Expiration { get; set; }
}