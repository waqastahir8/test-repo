using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class OrganizationsAndProjects : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_role_user_role_roles_id",
            schema: "users",
            table: "role_user");

        migrationBuilder.AddColumn<string>(
            name: "org_code",
            schema: "users",
            table: "user",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateTable(
            name: "organization",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                org_name = table.Column<string>(type: "text", nullable: false),
                org_code = table.Column<string>(type: "text", nullable: false),
                description = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_organization", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "project",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                project_name = table.Column<string>(type: "text", nullable: false),
                project_code = table.Column<string>(type: "text", nullable: false),
                project_type = table.Column<string>(type: "text", nullable: false),
                project_org = table.Column<string>(type: "text", nullable: false),
                description = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_project", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "userProject",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                project_name = table.Column<string>(type: "text", nullable: false),
                project_code = table.Column<string>(type: "text", nullable: false),
                project_type = table.Column<string>(type: "text", nullable: false),
                project_org = table.Column<string>(type: "text", nullable: false),
                active = table.Column<bool>(type: "boolean", nullable: false),
                user_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_project", x => x.id);
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
            name: "ix_user_user_project_users_id",
            schema: "users",
            table: "user_user_project",
            column: "users_id");

        migrationBuilder.AddForeignKey(
            name: "fk_role_user_roles_roles_id",
            schema: "users",
            table: "role_user",
            column: "roles_id",
            principalSchema: "users",
            principalTable: "role",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_role_user_roles_roles_id",
            schema: "users",
            table: "role_user");

        migrationBuilder.DropTable(
            name: "organization",
            schema: "users");

        migrationBuilder.DropTable(
            name: "project",
            schema: "users");

        migrationBuilder.DropTable(
            name: "user_user_project",
            schema: "users");

        migrationBuilder.DropTable(
            name: "userProject",
            schema: "users");

        migrationBuilder.DropColumn(
            name: "org_code",
            schema: "users",
            table: "user");

        migrationBuilder.AddForeignKey(
            name: "fk_role_user_role_roles_id",
            schema: "users",
            table: "role_user",
            column: "roles_id",
            principalSchema: "users",
            principalTable: "role",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }
}