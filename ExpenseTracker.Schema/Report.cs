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
//public int WeeklyApprovedExpenseCount { get; set; }
//public int WeeklyRejectedExpenseCount { get; set; }

//public int MonthlyApprovedExpenseCount { get; set; }
//public int MonthlyRejectedExpenseCount { get; set; }




// TODO: ReportCountRequest validator

public class ReportBreakdownRequest : BaseRequest
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public Guid UserId { get; set; }
    public string GroupBy { get; set; } // daily, weekly, monthly
}
public class ReportBreakdownResponse
{
    public string Period { get; set; } // Gün / Hafta / Ay bilgisi
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}



