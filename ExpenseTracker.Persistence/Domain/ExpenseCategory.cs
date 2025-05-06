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

        var insertedDate = DateTimeOffset.Parse("2025-04-22 19:00:00");
        var insertedUserId = Guid.Parse("9c0156ee-0027-40b2-af58-eb1c6cd8ddf0");

        builder.HasData(
            new ExpenseCategory
            {
                Id = Guid.Parse("16bcc643-b891-4f57-b9ec-ef136a7ac9b0"),
                Name = "transport",
                InsertedDate = insertedDate,
                InsertedUser = insertedUserId,
                IsActive = true
            },
            new ExpenseCategory
            {
                Id = Guid.Parse("62e9bb12-e245-49d0-9566-c6e4a59b4200"),
                Name = "Food",
                InsertedDate = insertedDate,
                InsertedUser = insertedUserId,
                IsActive = true
            }
        );
    }
}