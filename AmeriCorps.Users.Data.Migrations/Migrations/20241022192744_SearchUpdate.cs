#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class SearchUpdate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_operating_site_project_project_id",
            schema: "users",
            table: "operatingSite");

        migrationBuilder.DropForeignKey(
            name: "fk_project_award_award_id",
            schema: "users",
            table: "project");

        migrationBuilder.AlterColumn<int>(
            name: "award_id",
            schema: "users",
            table: "project",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "project_id",
            schema: "users",
            table: "operatingSite",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "fk_operating_site_project_project_id",
            schema: "users",
            table: "operatingSite",
            column: "project_id",
            principalSchema: "users",
            principalTable: "project",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "fk_project_award_award_id",
            schema: "users",
            table: "project",
            column: "award_id",
            principalSchema: "users",
            principalTable: "award",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_operating_site_project_project_id",
            schema: "users",
            table: "operatingSite");

        migrationBuilder.DropForeignKey(
            name: "fk_project_award_award_id",
            schema: "users",
            table: "project");

        migrationBuilder.AlterColumn<int>(
            name: "award_id",
            schema: "users",
            table: "project",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<int>(
            name: "project_id",
            schema: "users",
            table: "operatingSite",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AddForeignKey(
            name: "fk_operating_site_project_project_id",
            schema: "users",
            table: "operatingSite",
            column: "project_id",
            principalSchema: "users",
            principalTable: "project",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            name: "fk_project_award_award_id",
            schema: "users",
            table: "project",
            column: "award_id",
            principalSchema: "users",
            principalTable: "award",
            principalColumn: "id");
    }
}