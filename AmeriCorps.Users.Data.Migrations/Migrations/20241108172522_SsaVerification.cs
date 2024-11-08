using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SsaVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_org_code_user_account_status",
                schema: "users",
                table: "user");

            migrationBuilder.CreateTable(
                name: "socialSecurityVerification",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    citizenship_status = table.Column<int>(type: "integer", nullable: false),
                    social_security_status = table.Column<int>(type: "integer", nullable: false),
                    verification_code = table.Column<string>(type: "text", nullable: false),
                    citizenship_code = table.Column<string>(type: "text", nullable: false),
                    process_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    citizenship_updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    social_security_updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    submit_count = table.Column<int>(type: "integer", nullable: false),
                    last_submit_user = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_social_security_verification", x => x.id);
                    table.ForeignKey(
                        name: "fk_social_security_verification_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_org_code_user_account_status_encrypted_social_security",
                schema: "users",
                table: "user",
                columns: new[] { "org_code", "user_account_status", "encrypted_social_security_number" });

            migrationBuilder.CreateIndex(
                name: "ix_social_security_verification_user_id",
                schema: "users",
                table: "socialSecurityVerification",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "socialSecurityVerification",
                schema: "users");

            migrationBuilder.DropIndex(
                name: "ix_user_org_code_user_account_status_encrypted_social_security",
                schema: "users",
                table: "user");

            migrationBuilder.CreateIndex(
                name: "ix_user_org_code_user_account_status",
                schema: "users",
                table: "user",
                columns: new[] { "org_code", "user_account_status" });
        }
    }
}
