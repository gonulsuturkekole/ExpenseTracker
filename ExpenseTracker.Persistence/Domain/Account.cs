namespace ExpenseTracker.Persistence.Domain;

using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class Account : BaseEntity
{
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    public string Name { get; set; }
    public long AccountNumber { get; set; }
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
        builder.HasOne(x => x.User).WithOne(x => x.Account);

        var insertedDate = DateTimeOffset.Parse("2025-04-22 19:00:00");
        var accountNumber1 = 134268590;
        var accountNumber2 = 325652192;
        var systemGodUserId = Guid.Parse("c4ff8586-e24b-4338-9fd5-66f738fe181c");
        var gonulSuUserId = Guid.Parse("defa9635-caee-4682-86bb-c8624fc0488f");
        builder.HasData(new Account()
        {
            Id = Guid.Parse("79e18995-ac9a-4d16-848b-40d0b48df92c"),
            Name = "Papara Şirket Hesabı",
            OpenDate = insertedDate,
            UserId = systemGodUserId,
            IBAN = "TR" + accountNumber1.ToString("D20"),
            AccountNumber = accountNumber1,
            CurrencyCode = "TRY",
            Balance = 1000000,
            IsActive = true,
            InsertedDate = insertedDate,
        },
        new Account()
        {
            Id = Guid.Parse("16ee5456-47ec-4d8a-ad31-cca8bb558c47"),
            Name = "Papara Şirket Hesabı-2",
            OpenDate = insertedDate,
            UserId = gonulSuUserId,
            IBAN = "TR" + accountNumber2.ToString("D20"),
            AccountNumber = accountNumber2,
            CurrencyCode = "TRY",
            Balance = 1000000,
            IsActive = true,
            InsertedDate = insertedDate,
        });
    }
}