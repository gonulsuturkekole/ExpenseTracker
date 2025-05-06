namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;

public record ReportCountQuery(ReportCountRequest Model) : IRequest<ApiResponse<ReportCountResponse>>;

public record ReportBreakdownQuery(Guid personelId, ReportBreakdownRequest Model) : IRequest<ApiResponse<List<ReportBreakdownResponse>>>;

public record ReportPersonelCountQuery(ReportCountRequest Request) : IRequest<ApiResponse<ReportCountResponse>>;
