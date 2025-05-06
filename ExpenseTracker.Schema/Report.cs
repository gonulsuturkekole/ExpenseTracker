namespace ExpenseTracker.Schema;

using ExpenseTracker.Base;

public class ReportCountRequest : BaseRequest
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public Guid? UserId { get; set; } = null;
}

public class ReportCountResponse : BaseResponse
{
    public int ApprovedExpenseCount { get; set; }
    public int RejectedExpenseCount { get; set; }
    public decimal ApprovedExpenseAmount { get; set; }
    public Guid? UserId { get; set; }

}

public class ReportBreakdownRequest : BaseRequest
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public Guid UserId { get; set; }
    public string GroupBy { get; set; }
    

}

public class ReportBreakdownResponse : BaseResponse
{
    public string Period { get; set; }
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}


