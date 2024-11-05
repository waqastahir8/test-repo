#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations;
/// <inheritdoc />
public partial class SnakeCaseNamingConvention : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "Address",
            newName: "address",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "Attribute",
            newName: "attribute",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "CommunicationMethod",
            newName: "communication_method",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "Education",
            newName: "education",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "Language",
            newName: "language",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "MilitaryService",
            newName: "military_service",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "Reference",
            newName: "reference",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "Relative",
            newName: "relative",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "SavedSearch",
            newName: "saved_search",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "Skill",
            newName: "skill",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "User",
            newName: "user",
            schema: "users",
            newSchema: "users");

        // migrationBuilder.DropForeignKey(
        //     name: "FK_Reference_User_UserId",
        //     schema: "users",
        //     table: "reference");

        migrationBuilder.AlterColumn<int>(
            name: "UserId",
            schema: "users",
            table: "reference",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);

        // migrationBuilder.AddForeignKey(
        //     name: "fk_reference_user_user_id",
        //     schema: "users",
        //     table: "reference",
        //     column: "UserId",
        //     principalSchema: "users",
        //     principalTable: "user",
        //     principalColumn: "Id",
        //     onDelete: ReferentialAction.Cascade);

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "address",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "IsForeign",
            schema: "users",
            table: "address",
            newName: "is_foreign");

        migrationBuilder.RenameColumn(
            name: "Type",
            schema: "users",
            table: "address",
            newName: "type");

        migrationBuilder.RenameColumn(
            name: "Street1",
            schema: "users",
            table: "address",
            newName: "street1");

        migrationBuilder.RenameColumn(
            name: "Street2",
            schema: "users",
            table: "address",
            newName: "street2");

        migrationBuilder.RenameColumn(
            name: "City",
            schema: "users",
            table: "address",
            newName: "city");

        migrationBuilder.RenameColumn(
            name: "State",
            schema: "users",
            table: "address",
            newName: "state");

        migrationBuilder.RenameColumn(
            name: "Country",
            schema: "users",
            table: "address",
            newName: "country");

        migrationBuilder.RenameColumn(
            name: "ZipCode",
            schema: "users",
            table: "address",
            newName: "zip_code");

        migrationBuilder.RenameColumn(
            name: "MovingWithinSixMonths",
            schema: "users",
            table: "address",
            newName: "moving_within_six_months");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "address",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "attribute",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "Type",
            schema: "users",
            table: "attribute",
            newName: "type");

        migrationBuilder.RenameColumn(
            name: "Value",
            schema: "users",
            table: "attribute",
            newName: "value");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "attribute",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "communication_method",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "Type",
            schema: "users",
            table: "communication_method",
            newName: "type");

        migrationBuilder.RenameColumn(
            name: "Value",
            schema: "users",
            table: "communication_method",
            newName: "value");

        migrationBuilder.RenameColumn(
            name: "IsPreferred",
            schema: "users",
            table: "communication_method",
            newName: "is_preferred");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "communication_method",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "education",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "Level",
            schema: "users",
            table: "education",
            newName: "level");

        migrationBuilder.RenameColumn(
            name: "MajorAreaOfStudy",
            schema: "users",
            table: "education",
            newName: "major_area_of_study");

        migrationBuilder.RenameColumn(
            name: "Institution",
            schema: "users",
            table: "education",
            newName: "institution");

        migrationBuilder.RenameColumn(
            name: "City",
            schema: "users",
            table: "education",
            newName: "city");

        migrationBuilder.RenameColumn(
            name: "State",
            schema: "users",
            table: "education",
            newName: "state");

        migrationBuilder.RenameColumn(
            name: "DateAttendedFrom",
            schema: "users",
            table: "education",
            newName: "date_attended_from");

        migrationBuilder.RenameColumn(
            name: "DateAttendedTo",
            schema: "users",
            table: "education",
            newName: "date_attended_to");

        migrationBuilder.RenameColumn(
            name: "DegreeTypePursued",
            schema: "users",
            table: "education",
            newName: "degree_type_pursued");

        migrationBuilder.RenameColumn(
            name: "DegreeCompleted",
            schema: "users",
            table: "education",
            newName: "degree_completed");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "education",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "language",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "PickListId",
            schema: "users",
            table: "language",
            newName: "pick_list_id");

        migrationBuilder.RenameColumn(
            name: "IsPrimary",
            schema: "users",
            table: "language",
            newName: "is_primary");

        migrationBuilder.RenameColumn(
            name: "SpeakingAbility",
            schema: "users",
            table: "language",
            newName: "speaking_ability");

        migrationBuilder.RenameColumn(
            name: "WritingAbility",
            schema: "users",
            table: "language",
            newName: "writing_ability");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "language",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "military_service",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "PickListId",
            schema: "users",
            table: "military_service",
            newName: "pick_list_id");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "military_service",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "reference",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "TypeId",
            schema: "users",
            table: "reference",
            newName: "type_id");

        migrationBuilder.RenameColumn(
            name: "Relationship",
            schema: "users",
            table: "reference",
            newName: "relationship");

        migrationBuilder.RenameColumn(
            name: "RelationshipLength",
            schema: "users",
            table: "reference",
            newName: "relationship_length");

        migrationBuilder.RenameColumn(
            name: "ContactName",
            schema: "users",
            table: "reference",
            newName: "contact_name");

        migrationBuilder.RenameColumn(
            name: "Email",
            schema: "users",
            table: "reference",
            newName: "email");

        migrationBuilder.RenameColumn(
            name: "Phone",
            schema: "users",
            table: "reference",
            newName: "phone");

        migrationBuilder.RenameColumn(
            name: "Address",
            schema: "users",
            table: "reference",
            newName: "address");

        migrationBuilder.RenameColumn(
            name: "Company",
            schema: "users",
            table: "reference",
            newName: "company");

        migrationBuilder.RenameColumn(
            name: "Position",
            schema: "users",
            table: "reference",
            newName: "position");

        migrationBuilder.RenameColumn(
            name: "Notes",
            schema: "users",
            table: "reference",
            newName: "notes");

        migrationBuilder.RenameColumn(
            name: "CanContact",
            schema: "users",
            table: "reference",
            newName: "can_contact");

        migrationBuilder.RenameColumn(
            name: "Contacted",
            schema: "users",
            table: "reference",
            newName: "contacted");

        migrationBuilder.RenameColumn(
            name: "DateContacted",
            schema: "users",
            table: "reference",
            newName: "date_contacted");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "reference",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "relative",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "Relationship",
            schema: "users",
            table: "relative",
            newName: "relationship");

        migrationBuilder.RenameColumn(
            name: "HighestEducationLevel",
            schema: "users",
            table: "relative",
            newName: "highest_education_level");

        migrationBuilder.RenameColumn(
            name: "AnnualIncome",
            schema: "users",
            table: "relative",
            newName: "annual_income");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "relative",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "saved_search",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "Name",
            schema: "users",
            table: "saved_search",
            newName: "name");

        migrationBuilder.RenameColumn(
            name: "Filters",
            schema: "users",
            table: "saved_search",
            newName: "filters");

        migrationBuilder.RenameColumn(
            name: "NotificationsOn",
            schema: "users",
            table: "saved_search",
            newName: "notifications_on");

        migrationBuilder.RenameColumn(
            name: "LastRun",
            schema: "users",
            table: "saved_search",
            newName: "last_run");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "saved_search",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "skill",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "PickListId",
            schema: "users",
            table: "skill",
            newName: "pick_list_id");

        migrationBuilder.RenameColumn(
            name: "UserId",
            schema: "users",
            table: "skill",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "users",
            table: "user",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "Searchable",
            schema: "users",
            table: "user",
            newName: "searchable");

        migrationBuilder.RenameColumn(
            name: "UserName",
            schema: "users",
            table: "user",
            newName: "user_name");

        migrationBuilder.RenameColumn(
            name: "Prefix",
            schema: "users",
            table: "user",
            newName: "prefix");

        migrationBuilder.RenameColumn(
            name: "FirstName",
            schema: "users",
            table: "user",
            newName: "first_name");

        migrationBuilder.RenameColumn(
            name: "LastName",
            schema: "users",
            table: "user",
            newName: "last_name");

        migrationBuilder.RenameColumn(
            name: "MiddleName",
            schema: "users",
            table: "user",
            newName: "middle_name");

        migrationBuilder.RenameColumn(
            name: "PreferredName",
            schema: "users",
            table: "user",
            newName: "preferred_name");

        migrationBuilder.RenameColumn(
            name: "DateOfBirth",
            schema: "users",
            table: "user",
            newName: "date_of_birth");

        migrationBuilder.RenameColumn(
            name: "ExternalAccountId",
            schema: "users",
            table: "user",
            newName: "external_account_id");

    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "address",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "is_foreign",
            schema: "users",
            table: "address",
            newName: "IsForeign");

        migrationBuilder.RenameColumn(
            name: "type",
            schema: "users",
            table: "address",
            newName: "Type");

        migrationBuilder.RenameColumn(
            name: "street1",
            schema: "users",
            table: "address",
            newName: "Street1");

        migrationBuilder.RenameColumn(
            name: "street2",
            schema: "users",
            table: "address",
            newName: "Street2");

        migrationBuilder.RenameColumn(
            name: "city",
            schema: "users",
            table: "address",
            newName: "City");

        migrationBuilder.RenameColumn(
            name: "state",
            schema: "users",
            table: "address",
            newName: "State");

        migrationBuilder.RenameColumn(
            name: "country",
            schema: "users",
            table: "address",
            newName: "Country");

        migrationBuilder.RenameColumn(
            name: "zip_code",
            schema: "users",
            table: "address",
            newName: "ZipCode");

        migrationBuilder.RenameColumn(
            name: "moving_within_six_months",
            schema: "users",
            table: "address",
            newName: "MovingWithinSixMonths");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "address",
            newName: "UserId");

        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "attribute",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "type",
            schema: "users",
            table: "attribute",
            newName: "Type");

        migrationBuilder.RenameColumn(
            name: "value",
            schema: "users",
            table: "attribute",
            newName: "Value");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "attribute",
            newName: "UserId");

        //end rename user colums
        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "language",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "pick_list_id",
            schema: "users",
            table: "language",
            newName: "PickListId");

        migrationBuilder.RenameColumn(
            name: "is_primary",
            schema: "users",
            table: "language",
            newName: "IsPrimary");

        migrationBuilder.RenameColumn(
            name: "speaking_ability",
            schema: "users",
            table: "language",
            newName: "SpeakingAbility");

        migrationBuilder.RenameColumn(
            name: "writing_ability",
            schema: "users",
            table: "language",
            newName: "WritingAbility");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "language",
            newName: "UserId");

        //end language rename column
        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "military_service",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "pick_list_id",
            schema: "users",
            table: "military_service",
            newName: "PickListId");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "military_service",
            newName: "UserId");

        //end military service rename columns
        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "communication_method",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "type",
            schema: "users",
            table: "communication_method",
            newName: "Type");

        migrationBuilder.RenameColumn(
            name: "value",
            schema: "users",
            table: "communication_method",
            newName: "Value");

        migrationBuilder.RenameColumn(
            name: "is_preferred",
            schema: "users",
            table: "communication_method",
            newName: "IsPreferred");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "communication_method",
            newName: "UserId");

        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "education",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "level",
            schema: "users",
            table: "education",
            newName: "Level");

        migrationBuilder.RenameColumn(
            name: "major_area_of_study",
            schema: "users",
            table: "education",
            newName: "MajorAreaOfStudy");

        migrationBuilder.RenameColumn(
            name: "institution",
            schema: "users",
            table: "education",
            newName: "Institution");

        migrationBuilder.RenameColumn(
            name: "city",
            schema: "users",
            table: "education",
            newName: "City");

        migrationBuilder.RenameColumn(
            name: "state",
            schema: "users",
            table: "education",
            newName: "State");

        migrationBuilder.RenameColumn(
            name: "date_attended_from",
            schema: "users",
            table: "education",
            newName: "DateAttendedFrom");

        migrationBuilder.RenameColumn(
            name: "date_attended_to",
            schema: "users",
            table: "education",
            newName: "DateAttendedTo");

        migrationBuilder.RenameColumn(
            name: "degree_type_pursued",
            schema: "users",
            table: "education",
            newName: "DegreeTypePursued");

        migrationBuilder.RenameColumn(
            name: "degree_completed",
            schema: "users",
            table: "education",
            newName: "DegreeCompleted");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "education",
            newName: "UserId");

        //end rename of education columns
        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "reference",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "type_id",
            schema: "users",
            table: "reference",
            newName: "TypeId");

        migrationBuilder.RenameColumn(
            name: "relationship",
            schema: "users",
            table: "reference",
            newName: "Relationship");

        migrationBuilder.RenameColumn(
            name: "relationship_length",
            schema: "users",
            table: "reference",
            newName: "RelationshipLength");

        migrationBuilder.RenameColumn(
            name: "contact_name",
            schema: "users",
            table: "reference",
            newName: "ContactName");

        migrationBuilder.RenameColumn(
            name: "email",
            schema: "users",
            table: "reference",
            newName: "Email");

        migrationBuilder.RenameColumn(
            name: "phone",
            schema: "users",
            table: "reference",
            newName: "Phone");

        migrationBuilder.RenameColumn(
            name: "address",
            schema: "users",
            table: "reference",
            newName: "Address");

        migrationBuilder.RenameColumn(
            name: "company",
            schema: "users",
            table: "reference",
            newName: "Company");

        migrationBuilder.RenameColumn(
            name: "position",
            schema: "users",
            table: "reference",
            newName: "Position");

        migrationBuilder.RenameColumn(
            name: "notes",
            schema: "users",
            table: "reference",
            newName: "Notes");

        migrationBuilder.RenameColumn(
            name: "can_contact",
            schema: "users",
            table: "reference",
            newName: "CanContact");

        migrationBuilder.RenameColumn(
            name: "contacted",
            schema: "users",
            table: "reference",
            newName: "Contacted");

        migrationBuilder.RenameColumn(
            name: "date_contacted",
            schema: "users",
            table: "reference",
            newName: "DateContacted");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "reference",
            newName: "UserId");

        //end rename of reference columns

        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "relative",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "relationship",
            schema: "users",
            table: "relative",
            newName: "Relationship");

        migrationBuilder.RenameColumn(
            name: "highest_education_level",
            schema: "users",
            table: "relative",
            newName: "HighestEducationLevel");

        migrationBuilder.RenameColumn(
            name: "annual_income",
            schema: "users",
            table: "relative",
            newName: "AnnualIncome");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "relative",
            newName: "UserId");

        //end rename of relative columns

        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "saved_search",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "name",
            schema: "users",
            table: "saved_search",
            newName: "Name");

        migrationBuilder.RenameColumn(
            name: "filters",
            schema: "users",
            table: "saved_search",
            newName: "Filters");

        migrationBuilder.RenameColumn(
            name: "notifications_on",
            schema: "users",
            table: "saved_search",
            newName: "NotificationsOn");

        migrationBuilder.RenameColumn(
            name: "last_run",
            schema: "users",
            table: "saved_search",
            newName: "LastRun");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "saved_search",
            newName: "UserId");

        //end saved search rename columns
        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "skill",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "pick_list_id",
            schema: "users",
            table: "skill",
            newName: "PickListId");

        migrationBuilder.RenameColumn(
            name: "user_id",
            schema: "users",
            table: "skill",
            newName: "UserId");

        //end skill rename columns
        migrationBuilder.RenameColumn(
            name: "id",
            schema: "users",
            table: "user",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "searchable",
            schema: "users",
            table: "user",
            newName: "Searchable");

        migrationBuilder.RenameColumn(
            name: "user_name",
            schema: "users",
            table: "user",
            newName: "UserName");

        migrationBuilder.RenameColumn(
            name: "prefix",
            schema: "users",
            table: "user",
            newName: "Prefix");

        migrationBuilder.RenameColumn(
            name: "first_name",
            schema: "users",
            table: "user",
            newName: "FirstName");

        migrationBuilder.RenameColumn(
            name: "last_name",
            schema: "users",
            table: "user",
            newName: "LastName");

        migrationBuilder.RenameColumn(
            name: "middle_name",
            schema: "users",
            table: "user",
            newName: "MiddleName");

        migrationBuilder.RenameColumn(
            name: "preferred_name",
            schema: "users",
            table: "user",
            newName: "PreferredName");

        migrationBuilder.RenameColumn(
            name: "date_of_birth",
            schema: "users",
            table: "user",
            newName: "DateOfBirth");

        migrationBuilder.RenameColumn(
            name: "external_account_id",
            schema: "users",
            table: "user",
            newName: "ExternalAccountId");

        //end user rename columns
        migrationBuilder.RenameTable(
             name: "address",
             newName: "Address",
             schema: "users",
             newSchema: "users");

        migrationBuilder.RenameTable(
            name: "attribute",
            newName: "Attribute",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "communication_method",
            newName: "CommunicationMethod",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "education",
            newName: "Education",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "language",
            newName: "Language",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "military_service",
            newName: "MilitaryService",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "reference",
            newName: "Reference",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "relative",
            newName: "Relative",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "saved_search",
            newName: "SavedSearch",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "skill",
            newName: "Skill",
            schema: "users",
            newSchema: "users");

        migrationBuilder.RenameTable(
            name: "user",
            newName: "User",
            schema: "users",
            newSchema: "users");
    }
}