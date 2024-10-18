using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ProjectSearchFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                schema: "users",
                table: "project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_project_project_name_project_org_code_project_code_project_",
                schema: "users",
                table: "project",
                columns: new[] { "project_name", "project_org_code", "project_code", "project_id", "gsp_project_id", "program_name", "street_address", "city", "state", "project_type", "description" })
                .Annotation("Npgsql:IndexMethod", "GIST")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "ix_operating_site_program_year_operating_site_name_contact_name",
                schema: "users",
                table: "operatingSite",
                columns: new[] { "program_year", "operating_site_name", "contact_name", "email_address", "phone_number", "street_address", "street_address2", "city", "state", "zip_code" })
                .Annotation("Npgsql:IndexMethod", "GIST")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "ix_award_award_code_award_name_gsp_listing_number",
                schema: "users",
                table: "award",
                columns: new[] { "award_code", "award_name", "gsp_listing_number" })
                .Annotation("Npgsql:IndexMethod", "GIST")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_project_project_name_project_org_code_project_code_project_",
                schema: "users",
                table: "project");

            migrationBuilder.DropIndex(
                name: "ix_operating_site_program_year_operating_site_name_contact_name",
                schema: "users",
                table: "operatingSite");

            migrationBuilder.DropIndex(
                name: "ix_award_award_code_award_name_gsp_listing_number",
                schema: "users",
                table: "award");

            migrationBuilder.DropColumn(
                name: "active",
                schema: "users",
                table: "project");
        }
    }
}
