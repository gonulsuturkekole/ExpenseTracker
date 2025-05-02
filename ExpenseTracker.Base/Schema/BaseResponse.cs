namespace ExpenseTracker.Base;

public class BaseResponse
{
    public Guid Id { get; set; }
    public string InsertedUser { get; set; }
    public DateTimeOffset InsertedDate { get; set; }
    public string UpdatedUser { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}