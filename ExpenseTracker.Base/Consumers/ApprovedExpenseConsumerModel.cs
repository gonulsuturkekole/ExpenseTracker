namespace ExpenseTracker.Base.Consumers;

public class ApprovedExpenseConsumerModel
{
    public Guid ApprovedUserId { get; set; }
    public Guid ExpenseOwnerUserId { get; set; }
    public decimal Amount { get; set; }
}
