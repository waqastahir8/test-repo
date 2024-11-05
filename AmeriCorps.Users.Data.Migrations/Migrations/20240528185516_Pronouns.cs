#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class Pronouns : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Pronouns",
            schema: "users",
            table: "user",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.RenameColumn(
            name: "Pronouns",
            schema: "users",
            table: "user",
            newName: "pronouns"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "pronouns",
            schema: "users",
            table: "user");
    }
}