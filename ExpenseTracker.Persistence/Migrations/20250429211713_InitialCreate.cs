using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InsertedUser = table.Column<Guid>(type: "uuid", nullable: false),
                    InsertedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUser = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: true),
                    InsertedUser = table.Column<Guid>(type: "uuid", nullable: false),
                    InsertedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUser = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Secret = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    OpenDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastLoginDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    InsertedUser = table.Column<Guid>(type: "uuid", nullable: false),
                    InsertedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUser = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    AccountNumber = table.Column<int>(type: "integer", nullable: false),
                    IBAN = table.Column<string>(type: "text", nullable: true),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    OpenDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CloseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    InsertedUser = table.Column<Guid>(type: "uuid", nullable: false),
                    InsertedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUser = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RejectReason = table.Column<string>(type: "text", nullable: true),
                    InsertedUser = table.Column<Guid>(type: "uuid", nullable: false),
                    InsertedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUser = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DebitAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    CreditAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: true),
                    TransferType = table.Column<string>(type: "text", nullable: true),
                    InsertedUser = table.Column<Guid>(type: "uuid", nullable: false),
                    InsertedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUser = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    InsertedUser = table.Column<Guid>(type: "uuid", nullable: false),
                    InsertedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUser = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseDocuments_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_AccountId",
                table: "AccountTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseDocuments_ExpenseId",
                table: "ExpenseDocuments",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CategoryId",
                table: "Expenses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTransactions");

            migrationBuilder.DropTable(
                name: "ExpenseDocuments");

            migrationBuilder.DropTable(
                name: "MoneyTransfers");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
