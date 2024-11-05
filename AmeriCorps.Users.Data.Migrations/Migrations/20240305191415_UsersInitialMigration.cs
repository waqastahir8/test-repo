using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class UsersInitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "users");

        migrationBuilder.CreateTable(
            name: "User",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Searchable = table.Column<bool>(type: "boolean", nullable: false),
                UserName = table.Column<string>(type: "text", nullable: false),
                Prefix = table.Column<string>(type: "text", nullable: true),
                FirstName = table.Column<string>(type: "text", nullable: false),
                LastName = table.Column<string>(type: "text", nullable: false),
                MiddleName = table.Column<string>(type: "text", nullable: false),
                PreferredName = table.Column<string>(type: "text", nullable: false),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Address",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IsForeign = table.Column<bool>(type: "boolean", nullable: false),
                Type = table.Column<string>(type: "text", nullable: false),
                Street1 = table.Column<string>(type: "text", nullable: false),
                Street2 = table.Column<string>(type: "text", nullable: false),
                City = table.Column<string>(type: "text", nullable: false),
                State = table.Column<string>(type: "text", nullable: false),
                Country = table.Column<string>(type: "text", nullable: false),
                ZipCode = table.Column<string>(type: "text", nullable: false),
                MovingWithinSixMonths = table.Column<bool>(type: "boolean", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Address", x => x.Id);
                table.ForeignKey(
                    name: "FK_Address_User_UserId",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "User",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Attribute",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Type = table.Column<string>(type: "text", nullable: false),
                Value = table.Column<string>(type: "text", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Attribute", x => x.Id);
                table.ForeignKey(
                    name: "FK_Attribute_User_UserId",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "User",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "CommunicationMethod",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Type = table.Column<string>(type: "text", nullable: false),
                Value = table.Column<string>(type: "text", nullable: false),
                IsPreferred = table.Column<bool>(type: "boolean", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CommunicationMethod", x => x.Id);
                table.ForeignKey(
                    name: "FK_CommunicationMethod_User_UserId",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "User",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Education",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Level = table.Column<string>(type: "text", nullable: false),
                MajorAreaOfStudy = table.Column<string>(type: "text", nullable: false),
                Institution = table.Column<string>(type: "text", nullable: false),
                City = table.Column<string>(type: "text", nullable: false),
                State = table.Column<string>(type: "text", nullable: false),
                DateAttendedFrom = table.Column<DateOnly>(type: "date", nullable: false),
                DateAttendedTo = table.Column<DateOnly>(type: "date", nullable: false),
                DegreeTypePursued = table.Column<string>(type: "text", nullable: false),
                DegreeCompleted = table.Column<bool>(type: "boolean", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Education", x => x.Id);
                table.ForeignKey(
                    name: "FK_Education_User_UserId",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "User",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Language",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PickListId = table.Column<string>(type: "text", nullable: false),
                IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                SpeakingAbility = table.Column<string>(type: "text", nullable: false),
                WritingAbility = table.Column<string>(type: "text", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Language", x => x.Id);
                table.ForeignKey(
                    name: "FK_Language_User_UserId",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "User",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Relative",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Relationship = table.Column<string>(type: "text", nullable: false),
                HighestEducationLevel = table.Column<string>(type: "text", nullable: false),
                AnnualIncome = table.Column<int>(type: "integer", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Relative", x => x.Id);
                table.ForeignKey(
                    name: "FK_Relative_User_UserId",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "User",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Skill",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PickListId = table.Column<string>(type: "text", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Skill", x => x.Id);
                table.ForeignKey(
                    name: "FK_Skill_User_UserId",
                    column: x => x.UserId,
                    principalSchema: "users",
                    principalTable: "User",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Address_UserId",
            schema: "users",
            table: "Address",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Attribute_UserId",
            schema: "users",
            table: "Attribute",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_CommunicationMethod_UserId",
            schema: "users",
            table: "CommunicationMethod",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Education_UserId",
            schema: "users",
            table: "Education",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Language_UserId",
            schema: "users",
            table: "Language",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Relative_UserId",
            schema: "users",
            table: "Relative",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Skill_UserId",
            schema: "users",
            table: "Skill",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Address",
            schema: "users");

        migrationBuilder.DropTable(
            name: "Attribute",
            schema: "users");

        migrationBuilder.DropTable(
            name: "CommunicationMethod",
            schema: "users");

        migrationBuilder.DropTable(
            name: "Education",
            schema: "users");

        migrationBuilder.DropTable(
            name: "Language",
            schema: "users");

        migrationBuilder.DropTable(
            name: "Relative",
            schema: "users");

        migrationBuilder.DropTable(
            name: "Skill",
            schema: "users");

        migrationBuilder.DropTable(
            name: "User",
            schema: "users");
    }
}