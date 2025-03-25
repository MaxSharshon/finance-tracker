using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFinancialOperationCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                principalTable: "BalanceChanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                principalTable: "BalanceChanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
