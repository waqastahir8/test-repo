using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class IndexUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_user_org_code_user_account_status",
                schema: "users",
                table: "user",
                columns: new[] { "org_code", "user_account_status" });

            migrationBuilder.CreateIndex(
                name: "ix_role_role_name",
                schema: "users",
                table: "role",
                column: "role_name");

            migrationBuilder.CreateIndex(
                name: "ix_project_project_code",
                schema: "users",
                table: "project",
                column: "project_code");

            migrationBuilder.CreateIndex(
                name: "ix_organization_org_code",
                schema: "users",
                table: "organization",
                column: "org_code");

            migrationBuilder.CreateIndex(
                name: "ix_access_access_name",
                schema: "users",
                table: "access",
                column: "access_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_org_code_user_account_status",
                schema: "users",
                table: "user");

            migrationBuilder.DropIndex(
                name: "ix_role_role_name",
                schema: "users",
                table: "role");

            migrationBuilder.DropIndex(
                name: "ix_project_project_code",
                schema: "users",
                table: "project");

            migrationBuilder.DropIndex(
                name: "ix_organization_org_code",
                schema: "users",
                table: "organization");

            migrationBuilder.DropIndex(
                name: "ix_access_access_name",
                schema: "users",
                table: "access");
        }
    }
}
