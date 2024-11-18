using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;

public partial class MSYInfoUpdateToProjectOperatingSiteSubGrantee : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<double>(
            name: "awarded_msys",
            schema: "users",
            table: "sub_grantee",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "living_allowance_msys",
            schema: "users",
            table: "sub_grantee",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "non_living_allowance_msys",
            schema: "users",
            table: "sub_grantee",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "living_allowance_msys",
            schema: "users",
            table: "project",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "non_living_allowance_msys",
            schema: "users",
            table: "project",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "total_awarded_msys",
            schema: "users",
            table: "project",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "awarded_msys",
            schema: "users",
            table: "operatingSite",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "living_allowance_msys",
            schema: "users",
            table: "operatingSite",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "non_living_allowance_msys",
            schema: "users",
            table: "operatingSite",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "awarded_msys",
            schema: "users",
            table: "sub_grantee");

        migrationBuilder.DropColumn(
            name: "living_allowance_msys",
            schema: "users",
            table: "sub_grantee");

        migrationBuilder.DropColumn(
            name: "non_living_allowance_msys",
            schema: "users",
            table: "sub_grantee");

        migrationBuilder.DropColumn(
            name: "living_allowance_msys",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "non_living_allowance_msys",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "total_awarded_msys",
            schema: "users",
            table: "project");

        migrationBuilder.DropColumn(
            name: "awarded_msys",
            schema: "users",
            table: "operatingSite");

        migrationBuilder.DropColumn(
            name: "living_allowance_msys",
            schema: "users",
            table: "operatingSite");

        migrationBuilder.DropColumn(
            name: "non_living_allowance_msys",
            schema: "users",
            table: "operatingSite");
    }
}

