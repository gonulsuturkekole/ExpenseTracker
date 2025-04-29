namespace ExpenseTracker.Persistence.Domain;

using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class Account : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public string Name { get; set; }
    public int AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyCode { get; set; }
    public DateTimeOffset OpenDate { get; set; }
    public DateTimeOffset? CloseDate { get; set; }

    public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
}

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.AccountTransactions).WithOne(x => x.Account).HasForeignKey(x => x.AccountId);
    }
}