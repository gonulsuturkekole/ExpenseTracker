namespace ExpenseTracker.Business.Command;

using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Persistence.Domain;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class UserCommandHandler
    : IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>
{
    private readonly ExpenseTrackerDbContext _dbContext;

    public UserCommandHandler(ExpenseTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var isUserExists = await _dbContext.Users.AnyAsync(x => x.UserName == request.User.UserName);
        if (isUserExists)
        {
            return new ApiResponse<UserResponse>("user already exists");
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            UserName = request.User.UserName,
            FirstName = request.User.FirstName,
            LastName = request.User.LastName,
            IsActive = true,
            Secret = PasswordGenerator.GeneratePassword(30),
            OpenDate = DateTimeOffset.UtcNow,
            InsertedDate = DateTimeOffset.UtcNow,
            Role = request.User.Role,
        };

        user.Password = PasswordGenerator.CreateMD5(request.User.Password, user.Secret);

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<UserResponse>(new UserResponse()
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = true,
            Role = user.Role,
           
        });
    }
}