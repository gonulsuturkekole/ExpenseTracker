namespace ExpenseTracker.Schema;

using ExpenseTracker.Base;
using FluentValidation;

public class ExpenseCategoryRequest : BaseRequest
{
    public string Name { get; set; }
}

public class ExpenseCategoryRequestValidator : AbstractValidator<ExpenseCategoryRequest>
{
    public ExpenseCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(32);
    }
}

public class ExpenseCategoryResponse : BaseResponse
{
}