using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpenseTracker.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "expense_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    inserted_user = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_user = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_expense_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "money_transfers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    to_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    fee_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    transaction_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    reference_number = table.Column<string>(type: "text", nullable: true),
                    inserted_user = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_user = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_money_transfers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    secret = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    open_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    last_login_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role = table.Column<int>(type: "integer", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_user = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_user = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    account_number = table.Column<long>(type: "bigint", nullable: false),
                    iban = table.Column<string>(type: "text", nullable: true),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    currency_code = table.Column<string>(type: "text", nullable: true),
                    open_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    close_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    inserted_user = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_user = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.id);
                    table.ForeignKey(
                        name: "fk_accounts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    location = table.Column<string>(type: "text", nullable: true),
                    expense_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    reject_reason = table.Column<string>(type: "text", nullable: true),
                    inserted_user = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_user = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_expenses", x => x.id);
                    table.ForeignKey(
                        name: "fk_expenses_expense_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "expense_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_expenses_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    debit_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    credit_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    transaction_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    reference_number = table.Column<string>(type: "text", nullable: true),
                    transfer_type = table.Column<string>(type: "text", nullable: true),
                    inserted_user = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_user = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_transactions_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expense_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    expense_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    file_path = table.Column<string>(type: "text", nullable: true),
                    inserted_user = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_user = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_expense_documents", x => x.id);
                    table.ForeignKey(
                        name: "fk_expense_documents_expenses_expense_id",
                        column: x => x.expense_id,
                        principalTable: "expenses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "account_id", "first_name", "inserted_date", "inserted_user", "is_active", "last_login_date", "last_name", "open_date", "password", "role", "secret", "updated_date", "updated_user", "user_name" },
                values: new object[,]
                {
                    { new Guid("c4ff8586-e24b-4338-9fd5-66f738fe181c"), new Guid("00000000-0000-0000-0000-000000000000"), "System", new DateTimeOffset(new DateTime(2025, 4, 22, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("c4ff8586-e24b-4338-9fd5-66f738fe181c"), true, null, "God", new DateTimeOffset(new DateTime(2025, 4, 22, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "F16E5312912F730576C603E9E1AFB548", 0, "oLDacEmMM77Ud0MPqTEPfWpet2UijG", null, null, "systemgod" },
                    { new Guid("defa9635-caee-4682-86bb-c8624fc0488f"), new Guid("00000000-0000-0000-0000-000000000000"), "Gonul Su", new DateTimeOffset(new DateTime(2025, 4, 22, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("defa9635-caee-4682-86bb-c8624fc0488f"), true, null, "Turkekole", new DateTimeOffset(new DateTime(2025, 4, 22, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "DABE5A99E217242EDAEB0832AB06E261", 0, "a8CwnPbUZw1Q2fYnECcXI6tPgC1AJ5", null, null, "gonulsu" }
                });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "account_number", "balance", "close_date", "currency_code", "iban", "inserted_date", "inserted_user", "is_active", "name", "open_date", "updated_date", "updated_user", "user_id" },
                values: new object[,]
                {
                    { new Guid("16ee5456-47ec-4d8a-ad31-cca8bb558c47"), 325652192L, 1000000m, null, "TRY", "TR00000000000325652192", new DateTimeOffset(new DateTime(2025, 4, 22, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), true, "Papara Şirket Hesabı-2", new DateTimeOffset(new DateTime(2025, 4, 22, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, new Guid("defa9635-caee-4682-86bb-c8624fc0488f") },
                    { new Guid("79e18995-ac9a-4d16-848b-40d0b48df92c"), 134268590L, 1000000m, null, "TRY", "TR00000000000134268590", new DateTimeOffset(new DateTime(2025, 4, 22, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), true, "Papara Şirket Hesabı", new DateTimeOffset(new DateTime(2025, 4, 22, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, new Guid("c4ff8586-e24b-4338-9fd5-66f738fe181c") }
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_transactions_account_id",
                table: "account_transactions",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_user_id",
                table: "accounts",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_expense_documents_expense_id",
                table: "expense_documents",
                column: "expense_id");

            migrationBuilder.CreateIndex(
                name: "ix_expenses_category_id",
                table: "expenses",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_expenses_user_id",
                table: "expenses",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_transactions");

            migrationBuilder.DropTable(
                name: "expense_documents");

            migrationBuilder.DropTable(
                name: "money_transfers");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "expense_categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
