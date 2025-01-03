﻿#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class InvitedDate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "account_status",
            schema: "users",
            table: "user");

        migrationBuilder.AddColumn<DateTime>(
            name: "invite_date",
            schema: "users",
            table: "user",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "invite_user_id",
            schema: "users",
            table: "user",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "user_account_status",
            schema: "users",
            table: "user",
            type: "integer",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "invite_date",
            schema: "users",
            table: "user");

        migrationBuilder.DropColumn(
            name: "invite_user_id",
            schema: "users",
            table: "user");

        migrationBuilder.DropColumn(
            name: "user_account_status",
            schema: "users",
            table: "user");

        migrationBuilder.AddColumn<string>(
            name: "account_status",
            schema: "users",
            table: "user",
            type: "text",
            nullable: false,
            defaultValue: "");
    }
}