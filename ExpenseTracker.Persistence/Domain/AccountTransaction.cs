using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Domain;

public class AccountTransaction : BaseEntity
{
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }

    public string Description { get; set; }
    public decimal? DebitAmount { get; set; } // +++ 
    public decimal? CreditAmount { get; set; } // --- 
    public DateTimeOffset TransactionDate { get; set; }
    public string ReferenceNumber { get; set; }
    public string TransferType { get; set; }
}

public class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Account).WithMany(x => x.AccountTransactions).HasForeignKey(x => x.AccountId);
    }
}