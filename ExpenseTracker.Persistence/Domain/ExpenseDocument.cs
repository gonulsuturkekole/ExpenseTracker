using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Domain;

public class ExpenseDocument : BaseEntity
{
    public Guid ExpenseId { get; set; }
    public virtual Expense Expense { get; set; }

    public string FileName { get; set; }
    public string FilePath { get; set; }
}

public class ExpenseDocumentConfiguration : IEntityTypeConfiguration<ExpenseDocument>
{
    public void Configure(EntityTypeBuilder<ExpenseDocument> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Expense).WithMany(x => x.Documents).HasForeignKey(x => x.ExpenseId);
    }
}