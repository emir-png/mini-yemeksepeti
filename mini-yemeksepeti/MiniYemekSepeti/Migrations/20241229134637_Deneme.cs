using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniYemekSepeti.Migrations
{
    /// <inheritdoc />
    public partial class tryyy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Notifications",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "FoodName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ItemPrice",
                table: "Notifications",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemTotalAmount",
                table: "Notifications",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ItemPrice",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ItemTotalAmount",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
