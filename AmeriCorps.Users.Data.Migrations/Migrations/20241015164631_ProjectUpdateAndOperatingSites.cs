using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class ProjectUpdateAndOperatingSites : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "project_org",
            schema: "users",
            table: "project",
            newName: "program_year");

        migrationBuilder.AlterColumn<string>(
            name: "project_type",
            schema: "users",
            table: "project",
            type: "varchar(16)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "project_name",
            schema: "users",
            table: "project",
            type: "varchar(64)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "project_code",
            schema: "users",
            table: "project",
            type: "varchar(8)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "description",
            schema: "users",
            table: "project",
            type: "varchar(64)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AddColumn<int>(
            name: "authorized_rep_id",
            schema: "users",
            table: "project",
            type: "integer",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "award_id",
            schema: "users",
            table: "project",
            type: "integer",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "city",
            schema: "users",
            table: "project",
            type: "varchar(16)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateOnly>(
            name: "enrollment_end_dt",
            schema: "users",
            table: "project",
            type: "date",
            nullable: true);

        migrationBuilder.AddColumn<DateOnly>(
            name: "enrollment_start_dt",
            schema: "users",
            table: "project",
            type: "date",
            nullable: true);

        migrationBuilder.AddColumn<long>(
            name: "gsp_project_id",
            schema: "users",
            table: "project",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddColumn<string>(
            name: "program_name",
            schema: "users",
            table: "project",
            type: "varchar(64)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<int>(
            name: "project_director_id",
            schema: "users",
            table: "project",
            type: "integer",
            nullable: true);

        migrationBuilder.AddColumn<long>(
            name: "project_id",
            schema: "users",
            table: "project",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddColumn<string>(
            name: "project_org_code",
            schema: "users",
            table: "project",
            type: "varchar(8)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateOnly>(
            name: "project_period_end_dt",
            schema: "users",
            table: "project",
            type: "date",
            nullable: true);

        migrationBuilder.AddColumn<DateOnly>(
            name: "project_period_start_dt",
            schema: "users",
            table: "project",
            type: "date",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "state",
            schema: "users",
            table: "project",
            type: "varchar(8)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "street_address",
            schema: "users",
            table: "project",
            type: "varchar(64)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "zip_code",
            schema: "users",
            table: "project",
            type: "varchar(8)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateTable(
            name: "award",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                award_code = table.Column<string>(type: "varchar(16)", nullable: false),
                award_name = table.Column<string>(type: "varchar(64)", nullable: false),
                gsp_listing_number = table.Column<string>(type: "varchar(32)", nullable: false),
                fain = table.Column<long>(type: "bigint", nullable: false),
                uei = table.Column<long>(type: "bigint", nullable: false),
                performance_start_dt = table.Column<DateOnly>(type: "date", nullable: true),
                performance_end_dt = table.Column<DateOnly>(type: "date", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_award", x => x.id);
            });

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

        migrationBuilder.CreateTable(
            name: "sub_grantee",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                grantee_code = table.Column<string>(type: "varchar(32)", nullable: false),
                grantee_name = table.Column<string>(type: "varchar(64)", nullable: false),
                uei = table.Column<long>(type: "bigint", nullable: false),
                street_address = table.Column<string>(type: "varchar(32)", nullable: false),
                city = table.Column<string>(type: "varchar(32)", nullable: false),
                state = table.Column<string>(type: "varchar(8)", nullable: false),
                zip_code = table.Column<string>(type: "varchar(8)", nullable: false),
                project_id = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sub_grantee", x => x.id);
                table.ForeignKey(
                    name: "fk_sub_grantee_projects_project_id",
                    column: x => x.project_id,
                    principalSchema: "users",
                    principalTable: "project",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "ix_project_authorized_rep_id",
            schema: "users",
            table: "project",
            column: "authorized_rep_id");

        migrationBuilder.CreateIndex(
            name: "ix_project_award_id",
            schema: "users",
            table: "project",
            column: "award_id");

        migrationBuilder.CreateIndex(
            name: "ix_project_project_director_id",
            schema: "users",
            table: "project",
            column: "project_director_id");

        migrationBuilder.CreateIndex(
            name: "ix_award_award_code",
            schema: "users",
            table: "award",
            column: "award_code");

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

        migrationBuilder.CreateIndex(
            name: "ix_sub_grantee_grantee_code",
            schema: "users",
            table: "sub_grantee",
            column: "grantee_code");

        migrationBuilder.CreateIndex(
            name: "ix_sub_grantee_project_id",
            schema: "users",
            table: "sub_grantee",
            column: "project_id");

        migrationBuilder.AddForeignKey(
            name: "fk_project_award_award_id",
            schema: "users",
            table: "project",
            column: "award_id",
            principalSchema: "users",
            principalTable: "award",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            name: "fk_project_user_authorized_rep_id",
            schema: "users",
            table: "project",
            column: "authorized_rep_id",
            principalSchema: "users",
            principalTable: "user",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            name: "fk_project_user_project_director_id",
            schema: "users",
            table: "project",
            column: "project_director_id",
            principalSchema: "users",
            principalTable: "user",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_project_award_award_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropForeignKey(
            name: "fk_project_user_authorized_rep_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropForeignKey(
            name: "fk_project_user_project_director_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropTable(
            name: "award",
            schema: "users");

        migrationBuilder.DropTable(
            name: "operatingSite",
            schema: "users");

        migrationBuilder.DropTable(
            name: "sub_grantee",
            schema: "users");

        migrationBuilder.DropIndex(
            name: "ix_project_authorized_rep_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropIndex(
            name: "ix_project_award_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropIndex(
            name: "ix_project_project_director_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "authorized_rep_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "award_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "city",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "enrollment_end_dt",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "enrollment_start_dt",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "gsp_project_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "program_name",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "project_director_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "project_id",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "project_org_code",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "project_period_end_dt",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "project_period_start_dt",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "state",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "street_address",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "zip_code",
            schema: "users",
            table: "project");

        migrationBuilder.RenameColumn(
            name: "program_year",
            schema: "users",
            table: "project",
            newName: "project_org");

        migrationBuilder.AlterColumn<string>(
            name: "project_type",
            schema: "users",
            table: "project",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(16)");

        migrationBuilder.AlterColumn<string>(
            name: "project_name",
            schema: "users",
            table: "project",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)");

        migrationBuilder.AlterColumn<string>(
            name: "project_code",
            schema: "users",
            table: "project",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(8)");

        migrationBuilder.AlterColumn<string>(
            name: "description",
            schema: "users",
            table: "project",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)");
    }
}