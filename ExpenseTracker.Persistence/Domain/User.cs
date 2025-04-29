namespace ExpenseTracker.Persistence.Domain;

using ExpenseTracker.Base;
using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public enum UserRoles
{
    Admin,
    Personel
}

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

        

        
    }
}