using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UserModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "document_expiration_date",
                schema: "users",
                table: "user",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "resident_registration_number",
                schema: "users",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state_of_birth",
                schema: "users",
                table: "user",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "document_expiration_date",
                schema: "users",
                table: "user");

            migrationBuilder.DropColumn(
                name: "resident_registration_number",
                schema: "users",
                table: "user");

            migrationBuilder.DropColumn(
                name: "state_of_birth",
                schema: "users",
                table: "user");
        }
    }
}
