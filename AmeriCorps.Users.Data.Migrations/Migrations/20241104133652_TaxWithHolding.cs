using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;

public partial class TaxWithHolding : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_direct_deposit_user_id",
            schema: "users",
            table: "direct_deposit");

        migrationBuilder.CreateTable(
            name: "tax_with_holding",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                tax_with_holding_type = table.Column<int>(type: "integer", nullable: false),
                step2box1 = table.Column<string>(type: "text", nullable: true),
                step2box2 = table.Column<string>(type: "text", nullable: true),
                step3box1 = table.Column<string>(type: "text", nullable: true),
                step3box2 = table.Column<string>(type: "text", nullable: true),
                step4box1 = table.Column<string>(type: "text", nullable: true),
                step4box2 = table.Column<string>(type: "text", nullable: true),
                step4box3 = table.Column<string>(type: "text", nullable: true),
                user_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_tax_with_holding", x => x.id);
                table.ForeignKey(
                    name: "fk_tax_with_holding_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "users",
                    principalTable: "user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_direct_deposit_user_id",
            schema: "users",
            table: "direct_deposit",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_tax_with_holding_user_id",
            schema: "users",
            table: "tax_with_holding",
            column: "user_id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "tax_with_holding",
            schema: "users");

        migrationBuilder.DropIndex(
            name: "ix_direct_deposit_user_id",
            schema: "users",
            table: "direct_deposit");

        migrationBuilder.CreateIndex(
            name: "ix_direct_deposit_user_id",
            schema: "users",
            table: "direct_deposit",
            column: "user_id",
            unique: true);
    }
}
