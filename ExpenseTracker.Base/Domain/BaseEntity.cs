namespace ExpenseTracker.Base.Domain;

public class BaseEntity
{
    public Guid Id { get; set; }
    public Guid InsertedUser { get; set; }
    public DateTimeOffset InsertedDate { get; set; }
    public Guid? UpdatedUser { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}