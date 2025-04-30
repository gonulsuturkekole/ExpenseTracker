
using AutoMapper;
using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Persistence.Domain;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ExpenseTracker.Business.Query;

public class UserQueryHandler :
IRequestHandler<GetAllUsersQuery, ApiResponse<List<UserResponse>>>,
IRequestHandler<GetUserByIdQuery, ApiResponse<UserResponse>>
{
    private readonly ExpenseTrackerDbContext context;
    private readonly IMapper mapper;
    public UserQueryHandler(ExpenseTrackerDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<List<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var Users = await context.Set<User>().ToListAsync(cancellationToken);

        var mapped = mapper.Map<List<UserResponse>>(Users);
        return new ApiResponse<List<UserResponse>>(mapped);
    }

    public async Task<ApiResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<User>(true);
        predicate = predicate.And(x => x.Id == Guid.Parse(request.Id.ToString()));


        var User = await context.Set<User>().FirstOrDefaultAsync(predicate, cancellationToken);

        var mapped = mapper.Map<UserResponse>(User);
        return new ApiResponse<UserResponse>(mapped);
    }
}
