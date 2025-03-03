using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BalanceChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BalanceChangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialOperations_BalanceChanges_BalanceChangeId",
                        column: x => x.BalanceChangeId,
                        principalTable: "BalanceChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_BalanceChangeId",
                table: "FinancialOperations",
                column: "BalanceChangeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialOperations");

            migrationBuilder.DropTable(
                name: "BalanceChanges");
        }
    }
}
