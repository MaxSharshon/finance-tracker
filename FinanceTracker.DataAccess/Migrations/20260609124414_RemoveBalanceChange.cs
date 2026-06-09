using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBalanceChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations");

            migrationBuilder.DropTable(
                name: "BalanceChanges");

            migrationBuilder.DropIndex(
                name: "IX_FinancialOperations_BalanceChangeId",
                table: "FinancialOperations");

            migrationBuilder.DropColumn(
                name: "BalanceChangeId",
                table: "FinancialOperations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BalanceChangeId",
                table: "FinancialOperations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BalanceChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceChanges", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                unique: true,
                filter: "[BalanceChangeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                principalTable: "BalanceChanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
