namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;

public class ReportCountQuery : IRequest<ApiResponse<ReportCountResponse>>
{
    public ReportCountRequest Model { get; }

    public ReportCountQuery(ReportCountRequest model)
    {
        Model = model;
    }
}
public class ReportBreakdownQuery : IRequest<ApiResponse<List<ReportBreakdownResponse>>>
{
    public ReportBreakdownRequest Model { get; }

    public ReportBreakdownQuery(ReportBreakdownRequest model)
    {
        Model = model;
    }
}
public record ReportPersonelCountQuery(ReportCountRequest Request) : IRequest<ApiResponse<ReportCountResponse>>;
