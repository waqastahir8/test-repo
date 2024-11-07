#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class UserPPIUpdate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "city_of_birth",
            schema: "users",
            table: "user",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<int>(
            name: "citzen_ship_status",
            schema: "users",
            table: "user",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "country_of_birth",
            schema: "users",
            table: "user",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "encrypted_social_security_number",
            schema: "users",
            table: "user",
            type: "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "city_of_birth",
            schema: "users",
            table: "user");

        migrationBuilder.DropColumn(
            name: "citzen_ship_status",
            schema: "users",
            table: "user");

        migrationBuilder.DropColumn(
            name: "country_of_birth",
            schema: "users",
            table: "user");

        migrationBuilder.DropColumn(
            name: "encrypted_social_security_number",
            schema: "users",
            table: "user");
    }
}