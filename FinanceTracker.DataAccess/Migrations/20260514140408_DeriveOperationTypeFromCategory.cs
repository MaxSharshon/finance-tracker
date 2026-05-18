using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeriveOperationTypeFromCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_Categories_CategoryId",
                table: "FinancialOperations");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "FinancialOperations");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "FinancialOperations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_Categories_CategoryId",
                table: "FinancialOperations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialOperations_Categories_CategoryId",
                table: "FinancialOperations");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "FinancialOperations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "OperationType",
                table: "FinancialOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialOperations_Categories_CategoryId",
                table: "FinancialOperations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
