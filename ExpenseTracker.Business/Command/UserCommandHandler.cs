using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Persistence;
using ExpenseTracker.Persistence.Domain;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Business.Command;

public class UserCommandHandler :
    IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<DeleteUserCommand, ApiResponse>,
    IRequestHandler<UpdateUserCommand, ApiResponse<UserResponse>>
{
    private readonly ExpenseTrackerDbContext _dbContext;
    private readonly ICurrentUser _currentUser;

    public UserCommandHandler(ExpenseTrackerDbContext dbContext, ICurrentUser currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var isUserExists = await _dbContext.Users.AnyAsync(x => x.UserName == request.User.UserName);
        if (isUserExists)
        {
            return new ApiResponse<UserResponse>("user already exists");
        }

        var accountId = Guid.NewGuid();
        var user = new User
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
            InsertedUser = _currentUser.Id,
            AccountId = accountId,
        };

        user.Password = PasswordGenerator.CreateMD5(request.User.Password, user.Secret);

        await _dbContext.Users.AddAsync(user);

        var newAccount = new Account
        {
            Id = accountId,
            UserId = user.Id,
            Name = request.User.Account.Name,
            AccountNumber = request.User.Account.AccountNumber,
            IBAN = request.User.Account.IBAN,
            CurrencyCode = request.User.Account.CurrencyCode,
            OpenDate = DateTimeOffset.UtcNow,
            InsertedDate = DateTimeOffset.UtcNow,
            Balance = 0,
            IsActive = true,
            InsertedUser = _currentUser.Id,
        };
        await _dbContext.Accounts.AddAsync(newAccount);

        await _dbContext.SaveChangesAsync();

        return new ApiResponse<UserResponse>(new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = true,
            Role = user.Role,
            InsertedDate = user.InsertedDate
        });
    }

    public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (user == null)
        {
            return new ApiResponse("user not found");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse("user deleted successfully");
    }

    public async Task<ApiResponse<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (user == null)
        {
            return new ApiResponse<UserResponse>("user not found");
        }

        user.UserName = request.User.UserName;
        user.FirstName = request.User.FirstName;
        user.LastName = request.User.LastName;
        user.UpdatedDate = DateTimeOffset.UtcNow;
        user.UpdatedUser = _currentUser.Id;
        user.Role = request.User.Role;


        if (!string.IsNullOrWhiteSpace(request.User.Password))
        {
            user.Password = PasswordGenerator.CreateMD5(request.User.Password, user.Secret);
        }
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<UserResponse>(new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            IsActive = user.IsActive,
            InsertedDate = user.InsertedDate
        });
    }
}
