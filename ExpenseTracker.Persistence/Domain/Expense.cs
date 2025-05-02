
namespace ExpenseTracker.Persistence.Domain;

using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public enum ExpenseStatus
{
    Pending,
    Approved,
    Rejected
}

public class Expense : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public Guid CategoryId { get; set; }
    public virtual ExpenseCategory Category { get; set; }

    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTimeOffset ExpenseDate { get; set; }
    public ExpenseStatus Status { get; set; }
    public string RejectReason { get; set; }

    public virtual ICollection<ExpenseDocument> Documents { get; set; }
}

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.User).WithMany(x => x.Expenses).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Category).WithMany(x => x.Expenses).HasForeignKey(x => x.CategoryId);
        builder.HasMany(x => x.Documents).WithOne(x => x.Expense).HasForeignKey(x => x.ExpenseId);
    }
}
