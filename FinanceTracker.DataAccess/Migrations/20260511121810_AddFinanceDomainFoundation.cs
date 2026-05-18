using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFinanceDomainFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations");

            migrationBuilder.DropIndex(
                name: "IX_FinancialOperations_BalanceChangeId",
                table: "FinancialOperations");

            migrationBuilder.AlterColumn<Guid>(
                name: "BalanceChangeId",
                table: "FinancialOperations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "FinancialOperations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetId",
                table: "FinancialOperations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "FinancialOperations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FinancialOperations",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OperationType",
                table: "FinancialOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "FinancialOperations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budgets_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetUsers",
                columns: table => new
                {
                    BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetUsers", x => new { x.BudgetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_BudgetUsers_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationTags",
                columns: table => new
                {
                    FinancialOperationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTags", x => new { x.FinancialOperationId, x.TagId });
                    table.ForeignKey(
                        name: "FK_OperationTags_FinancialOperations_FinancialOperationId",
                        column: x => x.FinancialOperationId,
                        principalTable: "FinancialOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                unique: true,
                filter: "[BalanceChangeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_BudgetId",
                table: "FinancialOperations",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_CategoryId",
                table: "FinancialOperations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_UserId",
                table: "FinancialOperations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_OwnerUserId",
                table: "Budgets",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUsers_UserId",
                table: "BudgetUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId_Name_OperationType",
                table: "Categories",
                columns: new[] { "UserId", "Name", "OperationType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationTags_TagId",
                table: "OperationTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UserId_Name",
                table: "Tags",
                columns: new[] { "UserId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                principalTable: "BalanceChanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_Budgets_BudgetId",
                table: "FinancialOperations",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_Categories_CategoryId",
                table: "FinancialOperations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_Users_UserId",
                table: "FinancialOperations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_Budgets_BudgetId",
                table: "FinancialOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_Categories_CategoryId",
                table: "FinancialOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_Users_UserId",
                table: "FinancialOperations");

            migrationBuilder.DropTable(
                name: "BudgetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OperationTags");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_FinancialOperations_BalanceChangeId",
                table: "FinancialOperations");

            migrationBuilder.DropIndex(
                name: "IX_FinancialOperations_BudgetId",
                table: "FinancialOperations");

            migrationBuilder.DropIndex(
                name: "IX_FinancialOperations_CategoryId",
                table: "FinancialOperations");

            migrationBuilder.DropIndex(
                name: "IX_FinancialOperations_UserId",
                table: "FinancialOperations");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "FinancialOperations");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "FinancialOperations");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FinancialOperations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FinancialOperations");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "FinancialOperations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FinancialOperations");

            migrationBuilder.AlterColumn<Guid>(
                name: "BalanceChangeId",
                table: "FinancialOperations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                principalTable: "BalanceChanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
