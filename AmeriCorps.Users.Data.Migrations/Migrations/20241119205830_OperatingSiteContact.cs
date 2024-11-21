using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;

/// <inheritdoc />
public partial class OperatingSiteContact : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "contact_id",
            schema: "users",
            table: "operatingSite",
            type: "integer",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "ix_operating_site_contact_id",
            schema: "users",
            table: "operatingSite",
            column: "contact_id");

        migrationBuilder.AddForeignKey(
            name: "fk_operating_site_user_contact_id",
            schema: "users",
            table: "operatingSite",
            column: "contact_id",
            principalSchema: "users",
            principalTable: "user",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_operating_site_user_contact_id",
            schema: "users",
            table: "operatingSite");

        migrationBuilder.DropIndex(
            name: "ix_operating_site_contact_id",
            schema: "users",
            table: "operatingSite");

        migrationBuilder.DropColumn(
            name: "contact_id",
            schema: "users",
            table: "operatingSite");
    }
}
