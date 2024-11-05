using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class References : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Reference",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                TypeId = table.Column<string>(type: "text", nullable: false),
                Relationship = table.Column<string>(type: "text", nullable: false),
                RelationshipLength = table.Column<int>(type: "integer", nullable: false),
                ContactName = table.Column<string>(type: "text", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                Phone = table.Column<string>(type: "text", nullable: false),
                Address = table.Column<string>(type: "text", nullable: false),
                Company = table.Column<string>(type: "text", nullable: false),
                Position = table.Column<string>(type: "text", nullable: false),
                Notes = table.Column<string>(type: "text", nullable: false),
                CanContact = table.Column<bool>(type: "boolean", nullable: false),
                Contacted = table.Column<bool>(type: "boolean", nullable: false),
                DateContacted = table.Column<DateOnly>(type: "date", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reference", x => x.Id);
                table.ForeignKey(
                    name: "FK_Reference_User_UserId",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "User",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Reference_UserId",
            schema: "users",
            table: "Reference",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Reference",
            schema: "users");
    }
}