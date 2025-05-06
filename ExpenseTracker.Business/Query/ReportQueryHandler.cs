namespace ExpenseTracker.Business.Query;

using Dapper;
using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading;
using System.Threading.Tasks;

public class ReportQueryHandler :
    IRequestHandler<ReportCountQuery, ApiResponse<ReportCountResponse>>,
    IRequestHandler<ReportPersonelCountQuery, ApiResponse<ReportCountResponse>>,
    IRequestHandler<ReportBreakdownQuery, ApiResponse<IEnumerable<ReportBreakdownResponse>>>

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
            WHERE expense_date BETWEEN @StartDate AND @EndDate";

        var parameters = new DynamicParameters();
        parameters.Add("StartDate", request.Model.StartDate.UtcDateTime);
        parameters.Add("EndDate", request.Model.EndDate.UtcDateTime);

        var result = await _unitOfWork.QuerySingleAsync<ReportCountResponse>(sql, parameters);
        return new ApiResponse<ReportCountResponse>(result);
    }

    public async Task<ApiResponse<ReportCountResponse>> Handle(ReportPersonelCountQuery request, CancellationToken cancellationToken)
    {
        if (request.PersonelId == Guid.Empty)
        {
            return new ApiResponse<ReportCountResponse>("UserId is required for this report.");
        }

        var personel = await _unitOfWork.UserRepository.GetByIdAsync(request.PersonelId);
        if (personel == null)
        {
            return new ApiResponse<ReportCountResponse>("Personel not found");
        }
        else if (personel.Role == UserRoles.Admin)
        {
            return new ApiResponse<ReportCountResponse>("This report only work with personels");
        }

        var sql = @"
            SELECT
                COUNT(*) FILTER (WHERE status = 1) AS ApprovedExpenseCount,
                COUNT(*) FILTER (WHERE status = 2) AS RejectedExpenseCount,
                COALESCE(SUM(amount) FILTER (WHERE status = 1), 0) AS ApprovedExpenseAmount
            FROM expenses
            WHERE expense_date BETWEEN @StartDate AND @EndDate
            AND user_id = @UserId
        ";

        var parameters = new DynamicParameters();
        parameters.Add("StartDate", request.Model.StartDate.UtcDateTime);
        parameters.Add("EndDate", request.Model.EndDate.UtcDateTime);
        parameters.Add("UserId", request.PersonelId);

        var result = await _unitOfWork.QuerySingleAsync<ReportCountResponse>(sql, parameters);
        return new ApiResponse<ReportCountResponse>(result);
    }

    public async Task<ApiResponse<IEnumerable<ReportBreakdownResponse>>> Handle(ReportBreakdownQuery request, CancellationToken cancellationToken)
    {
        var sql = $@"
            SELECT
            u.first_name AS ""FirstName"",
            u.last_name AS ""LastName"",
            DATE_TRUNC(@GroupByInterval, e.expense_date) AS ""Period"",
            SUM(e.amount) AS ""TotalAmount"",
            json_agg(json_build_object(
                'Id', e.id,
                'Description', e.description,
                'Amount', e.amount,
                'ExpenseDate', e.expense_date
            ) ORDER BY e.expense_date) AS ""ExpenseDetailsJson""
            FROM expenses e
            JOIN users u ON e.user_id = u.id
            WHERE e.status = 1
            GROUP BY u.id, u.first_name, u.last_name, DATE_TRUNC(@GroupByInterval, e.expense_date)
            ORDER BY u.first_name, ""Period"";";

        var parameters = new DynamicParameters();
        parameters.Add("GroupByInterval", request.Model.GroupBy.ToString());

        var result = await _unitOfWork.QueryAsync<ReportBreakdownResponse>(sql, parameters);
        foreach (var item in result)
        {
            item.ExpenseDetails = System.Text.Json.JsonSerializer.Deserialize<List<ReportBreakdownExpenseDetail>>(item.ExpenseDetailsJson);
        }

        return new ApiResponse<IEnumerable<ReportBreakdownResponse>>(result);
    }

    
}

