using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class OperatingSites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operatingSite",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    program_year = table.Column<string>(type: "text", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    operating_site_name = table.Column<string>(type: "text", nullable: false),
                    contact_name = table.Column<string>(type: "text", nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    street_address = table.Column<string>(type: "text", nullable: false),
                    street_address2 = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    zip_code = table.Column<string>(type: "text", nullable: false),
                    plus4 = table.Column<string>(type: "text", nullable: false),
                    invite_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    invite_user_id = table.Column<int>(type: "integer", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    project_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operating_site", x => x.id);
                    table.ForeignKey(
                        name: "fk_operating_site_project_project_id",
                        column: x => x.project_id,
                        principalSchema: "users",
                        principalTable: "project",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_operating_site_operating_site_name",
                schema: "users",
                table: "operatingSite",
                column: "operating_site_name");

            migrationBuilder.CreateIndex(
                name: "ix_operating_site_project_id",
                schema: "users",
                table: "operatingSite",
                column: "project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "operatingSite",
                schema: "users");
        }
    }
}
