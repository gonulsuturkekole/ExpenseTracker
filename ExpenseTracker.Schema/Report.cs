namespace ExpenseTracker.Schema;

using ExpenseTracker.Base;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class ReportCountRequest : BaseRequest
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}

public class ReportCountRequestValidator : AbstractValidator<ReportCountRequest>
{
    public ReportCountRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.");
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be greater than start date.");
    }
}

public class ReportCountResponse 
{
    public int ApprovedExpenseCount { get; set; }
    public int RejectedExpenseCount { get; set; }
    public decimal ApprovedExpenseAmount { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReportBreakdownGroupBy
{
    Day,
    Week,
    Month,
}

public class ReportBreakdownRequest : BaseRequest
{
    public ReportBreakdownGroupBy? GroupBy { get; set; }
}

public class ReportBreakdownRequestValidator : AbstractValidator<ReportBreakdownRequest>
{
    public ReportBreakdownRequestValidator()
    {
        RuleFor(x => x.GroupBy).NotNull().WithMessage("GroupBy is required.");
        RuleFor(x => x.GroupBy).IsInEnum().WithMessage("GroupBy must be a valid enum value.");
    }
}

public class ReportBreakdownResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal TotalAmount { get; set; }

    [JsonIgnore]
    public string ExpenseDetailsJson { get; set; }

    [NotMapped]
    public List<ReportBreakdownExpenseDetail> ExpenseDetails { get; set; }
}

public class ReportBreakdownExpenseDetail
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset ExpenseDate { get; set; }
}
