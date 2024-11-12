using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SSAFileVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "file_status",
                schema: "users",
                table: "socialSecurityVerification",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ssa_verification_task_id",
                schema: "users",
                table: "socialSecurityVerification",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_status",
                schema: "users",
                table: "socialSecurityVerification");

            migrationBuilder.DropColumn(
                name: "ssa_verification_task_id",
                schema: "users",
                table: "socialSecurityVerification");
        }
    }
}
