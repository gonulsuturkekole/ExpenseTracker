namespace ExpenseTracker.Business.Query;

using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;


    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ApiResponse<IEnumerable<UserResponse>>>
    {
        private readonly ExpenseTrackerDbContext _dbContext;

        public GetAllUsersQueryHandler(ExpenseTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _dbContext.Users.ToListAsync(cancellationToken);

            var response = users.Select(user => new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OpenDate = user.OpenDate.DateTime,
                LastLoginDate = user.LastLoginDate?.DateTime
            });

            return new ApiResponse<IEnumerable<UserResponse>>(response);
        }
    }
