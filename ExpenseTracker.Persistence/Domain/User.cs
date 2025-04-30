namespace ExpenseTracker.Persistence.Domain;

using ExpenseTracker.Base;
using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Secret { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTimeOffset OpenDate { get; set; }
    public DateTimeOffset? LastLoginDate { get; set; }
    public UserRoles Role { get; set; }

    public virtual ICollection<Account> Accounts { get; set; }
    public virtual ICollection<Expense> Expenses { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Accounts).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasMany(x => x.Expenses).WithOne(x => x.User).HasForeignKey(x => x.UserId);

        var insertedDate = DateTimeOffset.Parse("2025-04-22 19:00:00");
        var systemGodUserId = Guid.Parse("c4ff8586-e24b-4338-9fd5-66f738fe181c");
        var systemGodSecret = PasswordGenerator.GeneratePassword(30);
        var systemGodPassword = PasswordGenerator.CreateMD5("a.A12345", systemGodSecret);
        builder.HasData(new User()
        {
            Id = systemGodUserId,
            UserName = "systemgod",
            Role = UserRoles.Admin,
            IsActive = true,
            InsertedDate = insertedDate,
            InsertedUser = systemGodUserId,
            FirstName = "System",
            LastName = "God",
            OpenDate = insertedDate,
            Secret = systemGodSecret,
            Password = systemGodPassword,
        });

        var gonulSuUserId = Guid.Parse("defa9635-caee-4682-86bb-c8624fc0488f");
        var gonulSuSecret = PasswordGenerator.GeneratePassword(30);
        var gonulSuPassword = PasswordGenerator.CreateMD5("a.A12345", gonulSuSecret);
        builder.HasData(new User()
        {
            Id = gonulSuUserId,
            UserName = "gonulsu",
            Role = UserRoles.Admin,
            IsActive = true,
            InsertedDate = insertedDate,
            InsertedUser = gonulSuUserId,
            FirstName = "Gonul Su",
            LastName = "Turkekole",
            OpenDate = insertedDate,
            Secret = gonulSuSecret,
            Password = gonulSuPassword,
        });
    }
}