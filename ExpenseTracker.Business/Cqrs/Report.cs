namespace ExpenseTracker.Business.Cqrs;

using ExpenseTracker.Base;
using ExpenseTracker.Schema;
using MediatR;

public record ReportCountQuery(ReportCountRequest Model) : IRequest<ApiResponse<ReportCountResponse>>;

public record ReportBreakdownQuery(ReportBreakdownRequest Model) : IRequest<ApiResponse<IEnumerable<ReportBreakdownResponse>>>;

public record ReportPersonelCountQuery(Guid PersonelId, ReportCountRequest Model) : IRequest<ApiResponse<ReportCountResponse>>;
