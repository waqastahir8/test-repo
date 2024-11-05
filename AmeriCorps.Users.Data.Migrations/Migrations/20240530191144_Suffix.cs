#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class Suffix : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Suffix",
            schema: "users",
            table: "user",
            type: "text",
            nullable: true);

        migrationBuilder.RenameColumn(
            name: "Suffix",
            schema: "users",
            table: "user",
            newName: "suffix"
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "suffix",
            schema: "users",
            table: "user");
    }

}