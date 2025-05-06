namespace ExpenseTracker.Schema;

using ExpenseTracker.Base;
using FluentValidation;
using Microsoft.AspNetCore.Http;

public class ExpenseGetRequest : BaseRequest
{
    public ExpenseStatus? Status { get; set; }
}

public class ExpenseRequest : BaseRequest
{
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTimeOffset ExpenseDate { get; set; }
    public IFormFile File { get; set; }
}

public class ExpenseRequestValidator : AbstractValidator<ExpenseRequest>
{
    public ExpenseRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Location).NotEmpty();
        RuleFor(x => x.ExpenseDate).LessThanOrEqualTo(DateTimeOffset.Now);
        RuleFor(x => x.File).NotNull();
    }
}

public class ExpenseResponse : BaseResponse
{
    public Guid UserId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Location { get; set; }
    public DateTimeOffset ExpenseDate { get; set; }
    public string CategoryName { get; set; }
    public ExpenseStatus Status { get; set; }
    public string RejectReason { get; set; }
}

public class UpdateExpenseStatusRequest : BaseRequest
{
    public ExpenseStatus Status { get; set; }
    public string RejectReason { get; set; }
}

public class UpdateExpenseStatusRequestValidator : AbstractValidator<UpdateExpenseStatusRequest>
{
    public UpdateExpenseStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.RejectReason).NotEmpty().When(x => x.Status == ExpenseStatus.Rejected);
        RuleFor(x => x.RejectReason).Empty().When(x => x.Status != ExpenseStatus.Rejected);
    }
}
