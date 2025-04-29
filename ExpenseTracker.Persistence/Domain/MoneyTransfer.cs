namespace ExpenseTracker.Persistence.Domain;

using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MoneyTransfer : BaseEntity
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public decimal? FeeAmount { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public string ReferenceNumber { get; set; }
}

public class MoneyTransferConfiguration : IEntityTypeConfiguration<MoneyTransfer>
{
    public void Configure(EntityTypeBuilder<MoneyTransfer> builder)
    {
        builder.HasKey(x => x.Id);
    }
}