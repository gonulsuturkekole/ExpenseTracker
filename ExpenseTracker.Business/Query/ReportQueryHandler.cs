namespace ExpenseTracker.Business.Query;

using Dapper;
using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

/*public class ReportQueryHandler
    : IRequestHandler<ReportCountQuery, ApiResponse<ReportCountResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<ApiResponse<ReportCountResponse>> Handle(ReportCountQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}*/

public class ReportQueryHandler :
    IRequestHandler<ReportCountQuery, ApiResponse<ReportCountResponse>>,
    IRequestHandler<ReportBreakdownQuery, ApiResponse<List<ReportBreakdownResponse>>> 

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public ReportQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<ApiResponse<ReportCountResponse>> Handle(ReportCountQuery request, CancellationToken cancellationToken)
    {
        var sql = @"
            SELECT
                COUNT(*) FILTER (WHERE status = 1) AS ApprovedExpenseCount,
                COUNT(*) FILTER (WHERE status = 2) AS RejectedExpenseCount,
                COALESCE(SUM(amount) FILTER (WHERE status = 1), 0) AS ApprovedExpenseAmount
            FROM expenses
            WHERE inserted_date BETWEEN @StartDate AND @EndDate
            /**userFilter**/";

        var parameters = new DynamicParameters();
        parameters.Add("StartDate", request.Model.StartDate.UtcDateTime);
        parameters.Add("EndDate", request.Model.EndDate.UtcDateTime);

        if (request.Model.UserId.HasValue && request.Model.UserId != Guid.Empty)
        {
            sql = sql.Replace("/**userFilter**/", "AND user_id = @UserId");
            parameters.Add("UserId", request.Model.UserId);
        }
        else
        {
            sql = sql.Replace("/**userFilter**/", "");
        }

        var result = await _unitOfWork.QuerySingleAsync<ReportCountResponse>(sql, parameters);
        return new ApiResponse<ReportCountResponse>(result);
    }

    public async Task<ApiResponse<List<ReportBreakdownResponse>>> Handle(ReportBreakdownQuery request, CancellationToken cancellationToken)
    {
        var sql = $@"
            SELECT 
                TO_CHAR(inserted_date, '{GetGroupingFormat(request.Model.GroupBy)}') AS Period,
                COUNT(*) AS Count,
                COALESCE(SUM(amount), 0) AS TotalAmount
            FROM expenses
            WHERE inserted_date BETWEEN @StartDate AND @EndDate
              AND user_id = @UserId
              AND status = 1
            GROUP BY Period
            ORDER BY Period;";

        var parameters = new DynamicParameters();
        parameters.Add("StartDate", request.Model.StartDate.UtcDateTime);
        parameters.Add("EndDate", request.Model.EndDate.UtcDateTime);
        parameters.Add("UserId", request.Model.UserId);

        var connectionString = _configuration.GetConnectionString("ExpenseTracker");
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();


        var result = (await connection.QueryAsync<ReportBreakdownResponse>(sql, parameters)).ToList();
        return new ApiResponse<List<ReportBreakdownResponse>>(result);
    }

    private string GetGroupingFormat(string groupBy)
    {
        return groupBy switch
        {
            "daily" => "YYYY-MM-DD",
            "weekly" => "IYYY-IW",     // ISO Week Number
            "monthly" => "YYYY-MM",
            _ => "YYYY-MM-DD"
        };
    }

}

