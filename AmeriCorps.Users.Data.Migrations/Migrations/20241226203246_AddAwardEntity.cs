using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddAwardEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_project_award_award_id",
                schema: "users",
                table: "project");

            migrationBuilder.DropPrimaryKey(
                name: "pk_award",
                schema: "users",
                table: "award");

            migrationBuilder.RenameTable(
                name: "award",
                schema: "users",
                newName: "awards",
                newSchema: "users");

            migrationBuilder.RenameIndex(
                name: "ix_award_award_code_award_name_gsp_listing_number",
                schema: "users",
                table: "awards",
                newName: "ix_awards_award_code_award_name_gsp_listing_number");

            migrationBuilder.RenameIndex(
                name: "ix_award_award_code",
                schema: "users",
                table: "awards",
                newName: "ix_awards_award_code");

            migrationBuilder.AddPrimaryKey(
                name: "pk_awards",
                schema: "users",
                table: "awards",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_project_awards_award_id",
                schema: "users",
                table: "project",
                column: "award_id",
                principalSchema: "users",
                principalTable: "awards",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_project_awards_award_id",
                schema: "users",
                table: "project");

            migrationBuilder.DropPrimaryKey(
                name: "pk_awards",
                schema: "users",
                table: "awards");

            migrationBuilder.RenameTable(
                name: "awards",
                schema: "users",
                newName: "award",
                newSchema: "users");

            migrationBuilder.RenameIndex(
                name: "ix_awards_award_code_award_name_gsp_listing_number",
                schema: "users",
                table: "award",
                newName: "ix_award_award_code_award_name_gsp_listing_number");

            migrationBuilder.RenameIndex(
                name: "ix_awards_award_code",
                schema: "users",
                table: "award",
                newName: "ix_award_award_code");

            migrationBuilder.AddPrimaryKey(
                name: "pk_award",
                schema: "users",
                table: "award",
                column: "id");

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
    }
}
