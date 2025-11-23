using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniYemekSepeti.Migrations
{
    /// <inheritdoc />
    public partial class AddIsReadToNotifications : Migration
    {
    

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        

    }
}
