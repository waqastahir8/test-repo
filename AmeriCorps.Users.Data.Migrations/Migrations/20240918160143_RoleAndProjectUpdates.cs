using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RoleAndProjectUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_user",
                schema: "users");

            migrationBuilder.DropTable(
                name: "user_user_project",
                schema: "users");

            migrationBuilder.RenameColumn(
                name: "fucntional_name",
                schema: "users",
                table: "role",
                newName: "role_type");

            migrationBuilder.AddColumn<string>(
                name: "functional_name",
                schema: "users",
                table: "role",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "access",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    access_name = table.Column<string>(type: "text", nullable: false),
                    access_level = table.Column<int>(type: "integer", nullable: false),
                    access_type = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_access", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project_access",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    access_name = table.Column<string>(type: "text", nullable: false),
                    access_level = table.Column<int>(type: "integer", nullable: false),
                    user_project_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_access", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_access_user_projects_user_project_id",
                        column: x => x.user_project_id,
                        principalSchema: "users",
                        principalTable: "userProject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_role",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "text", nullable: false),
                    functional_name = table.Column<string>(type: "text", nullable: false),
                    user_project_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_role_user_projects_user_project_id",
                        column: x => x.user_project_id,
                        principalSchema: "users",
                        principalTable: "userProject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "text", nullable: false),
                    functional_name = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_project_user_id",
                schema: "users",
                table: "userProject",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_access_user_project_id",
                schema: "users",
                table: "project_access",
                column: "user_project_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_role_user_project_id",
                schema: "users",
                table: "project_role",
                column: "user_project_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_user_id",
                schema: "users",
                table: "user_role",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_project_user_user_id",
                schema: "users",
                table: "userProject",
                column: "user_id",
                principalSchema: "users",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_project_user_user_id",
                schema: "users",
                table: "userProject");

            migrationBuilder.DropTable(
                name: "access",
                schema: "users");

            migrationBuilder.DropTable(
                name: "project_access",
                schema: "users");

            migrationBuilder.DropTable(
                name: "project_role",
                schema: "users");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "users");

            migrationBuilder.DropIndex(
                name: "ix_user_project_user_id",
                schema: "users",
                table: "userProject");

            migrationBuilder.DropColumn(
                name: "functional_name",
                schema: "users",
                table: "role");

            migrationBuilder.RenameColumn(
                name: "role_type",
                schema: "users",
                table: "role",
                newName: "fucntional_name");

            migrationBuilder.CreateTable(
                name: "role_user",
                schema: "users",
                columns: table => new
                {
                    roles_id = table.Column<int>(type: "integer", nullable: false),
                    users_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_user", x => new { x.roles_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_role_user_roles_roles_id",
                        column: x => x.roles_id,
                        principalSchema: "users",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_user_users_users_id",
                        column: x => x.users_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_user_project",
                schema: "users",
                columns: table => new
                {
                    user_projects_id = table.Column<int>(type: "integer", nullable: false),
                    users_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_user_project", x => new { x.user_projects_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_user_user_project_user_projects_user_projects_id",
                        column: x => x.user_projects_id,
                        principalSchema: "users",
                        principalTable: "userProject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_user_project_users_users_id",
                        column: x => x.users_id,
                        principalSchema: "users",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_role_user_users_id",
                schema: "users",
                table: "role_user",
                column: "users_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_user_project_users_id",
                schema: "users",
                table: "user_user_project",
                column: "users_id");
        }
    }
}
