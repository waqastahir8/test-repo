using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;

public partial class TaxWithHoldingHistoryUpdate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "step4box3",
            schema: "users",
            table: "tax_with_holding",
            newName: "other_income");

        migrationBuilder.RenameColumn(
            name: "step4box2",
            schema: "users",
            table: "tax_with_holding",
            newName: "extra_with_holding_amount");

        migrationBuilder.RenameColumn(
            name: "step4box1",
            schema: "users",
            table: "tax_with_holding",
            newName: "dependents_under17");

        migrationBuilder.RenameColumn(
            name: "step3box2",
            schema: "users",
            table: "tax_with_holding",
            newName: "dependents_over17");

        migrationBuilder.RenameColumn(
            name: "step3box1",
            schema: "users",
            table: "tax_with_holding",
            newName: "deductions");

        migrationBuilder.RenameColumn(
            name: "step2box2",
            schema: "users",
            table: "tax_with_holding",
            newName: "additional_with_holdings2");

        migrationBuilder.RenameColumn(
            name: "step2box1",
            schema: "users",
            table: "tax_with_holding",
            newName: "additional_with_holdings");

        migrationBuilder.AddColumn<DateTime>(
            name: "modified_date",
            schema: "users",
            table: "tax_with_holding",
            type: "timestamp with time zone",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "modified_date",
            schema: "users",
            table: "tax_with_holding");

        migrationBuilder.RenameColumn(
            name: "other_income",
            schema: "users",
            table: "tax_with_holding",
            newName: "step4box3");

        migrationBuilder.RenameColumn(
            name: "extra_with_holding_amount",
            schema: "users",
            table: "tax_with_holding",
            newName: "step4box2");

        migrationBuilder.RenameColumn(
            name: "dependents_under17",
            schema: "users",
            table: "tax_with_holding",
            newName: "step4box1");

        migrationBuilder.RenameColumn(
            name: "dependents_over17",
            schema: "users",
            table: "tax_with_holding",
            newName: "step3box2");

        migrationBuilder.RenameColumn(
            name: "deductions",
            schema: "users",
            table: "tax_with_holding",
            newName: "step3box1");

        migrationBuilder.RenameColumn(
            name: "additional_with_holdings2",
            schema: "users",
            table: "tax_with_holding",
            newName: "step2box2");

        migrationBuilder.RenameColumn(
            name: "additional_with_holdings",
            schema: "users",
            table: "tax_with_holding",
            newName: "step2box1");
    }
}
