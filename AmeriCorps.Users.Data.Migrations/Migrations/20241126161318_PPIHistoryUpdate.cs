using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;

    public partial class PPIHistoryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city_of_birth",
                schema: "users",
                table: "user");

            migrationBuilder.DropColumn(
                name: "country_of_birth",
                schema: "users",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "state_of_birth",
                schema: "users",
                table: "user",
                newName: "ppi_update_note");

            migrationBuilder.CreateTable(
                name: "city_of_birth",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    birth_city = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_city_of_birth", x => x.id);
                    table.ForeignKey(
                        name: "fk_city_of_birth_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "country_of_birth",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    birth_country = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country_of_birth", x => x.id);
                    table.ForeignKey(
                        name: "fk_country_of_birth_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "date_of_birth",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_date_of_birth", x => x.id);
                    table.ForeignKey(
                        name: "fk_date_of_birth_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "encrypted_social_security_number",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    socia_security_number = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_encrypted_social_security_number", x => x.id);
                    table.ForeignKey(
                        name: "fk_encrypted_social_security_number_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "state_of_birth",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    birth_state = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_state_of_birth", x => x.id);
                    table.ForeignKey(
                        name: "fk_state_of_birth_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_city_of_birth_user_id",
                schema: "users",
                table: "city_of_birth",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_country_of_birth_user_id",
                schema: "users",
                table: "country_of_birth",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_date_of_birth_user_id",
                schema: "users",
                table: "date_of_birth",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_encrypted_social_security_number_user_id",
                schema: "users",
                table: "encrypted_social_security_number",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_state_of_birth_user_id",
                schema: "users",
                table: "state_of_birth",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "city_of_birth",
                schema: "users");

            migrationBuilder.DropTable(
                name: "country_of_birth",
                schema: "users");

            migrationBuilder.DropTable(
                name: "date_of_birth",
                schema: "users");

            migrationBuilder.DropTable(
                name: "encrypted_social_security_number",
                schema: "users");

            migrationBuilder.DropTable(
                name: "state_of_birth",
                schema: "users");

            migrationBuilder.RenameColumn(
                name: "ppi_update_note",
                schema: "users",
                table: "user",
                newName: "state_of_birth");

            migrationBuilder.AddColumn<string>(
                name: "city_of_birth",
                schema: "users",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "country_of_birth",
                schema: "users",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    
}
