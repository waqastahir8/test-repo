using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class Collection : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "collection",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<int>(type: "integer", nullable: false),
                ListingId = table.Column<int>(type: "integer", nullable: false),
                Type = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_collection", x => x.Id);
                table.ForeignKey(
                    name: "fk_collection_user_user_id",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_collection_userId",
            schema: "users",
            table: "collection",
            column: "UserId");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "collection",
            newName: "id"
        );

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "collection",
            newName: "user_id"
        );

        migrationBuilder.RenameColumn(
            name: "ListingId",
            schema: "users",
            table: "collection",
            newName: "listing_id"
        );

        migrationBuilder.RenameColumn(
            name: "Type",
            schema: "users",
            table: "collection",
            newName: "type"
        );

    }
    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "collection",
            schema: "users");
    }
}