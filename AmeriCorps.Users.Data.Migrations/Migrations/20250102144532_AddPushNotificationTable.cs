using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;

/// <inheritdoc />
public partial class AddPushNotificationTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
        name: "push_notification",
        schema: "users",
        columns: table => new
        {
            id = table.Column<int>(nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.SerialColumn),
            user_id = table.Column<int>(nullable: false),
            title = table.Column<string>(maxLength: 255, nullable: false),
            message = table.Column<string>(type: "text", nullable: false),
            notification_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            notification_category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
            created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()") 
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_push_notification", x => x.id);
        });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "push_notification",
            schema: "users");
    }
}
