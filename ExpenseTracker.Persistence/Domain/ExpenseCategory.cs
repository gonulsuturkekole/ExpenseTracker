namespace ExpenseTracker.Persistence.Domain;

using ExpenseTracker.Base.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class ExpenseCategory : BaseEntity
{
    public string Name { get; set; }

    public ICollection<Expense> Expenses { get; set; }
}

public class ExpenseCategoryDocumentConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Expenses).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId);
    }
}