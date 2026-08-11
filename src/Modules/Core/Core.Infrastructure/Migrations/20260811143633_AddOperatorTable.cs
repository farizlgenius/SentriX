using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_role_guid",
                schema: "core",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_role_guid",
                schema: "core",
                table: "Users");

            migrationBuilder.DeleteData(
                schema: "core",
                table: "Locations",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "core",
                table: "Users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "location_guid",
                schema: "core",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "roleid",
                schema: "core",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "location_guid",
                schema: "core",
                table: "Roles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "location_guid",
                schema: "core",
                table: "Companies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "locationid",
                schema: "core",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Operators",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    active_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    role_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.id);
                    table.UniqueConstraint("AK_Operators_guid", x => x.guid);
                    table.ForeignKey(
                        name: "FK_Operators_Roles_role_guid",
                        column: x => x.role_guid,
                        principalSchema: "core",
                        principalTable: "Roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OperatorLocation",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operator_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    location_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorLocation", x => x.id);
                    table.ForeignKey(
                        name: "FK_OperatorLocation_Locations_location_guid",
                        column: x => x.location_guid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperatorLocation_Operators_operator_guid",
                        column: x => x.operator_guid,
                        principalSchema: "core",
                        principalTable: "Operators",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Locations",
                columns: new[] { "id", "country_id", "description", "guid", "is_active", "is_default", "name" },
                values: new object[] { 1, 178, "Main location", new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6"), true, true, "Main Location" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Operators",
                columns: new[] { "id", "active_time", "email", "expire_time", "is_active", "is_default", "password", "phone", "role_guid", "username" },
                values: new object[] { 1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "support@sentrix.com", new DateTime(9999, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=", "", new Guid("fe527691-7b13-4294-98b5-cb95181f5453"), "admin" });

            migrationBuilder.UpdateData(
                schema: "core",
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "is_default", "location_guid" },
                values: new object[] { true, new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") });

            migrationBuilder.CreateIndex(
                name: "IX_Users_location_guid",
                schema: "core",
                table: "Users",
                column: "location_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleid",
                schema: "core",
                table: "Users",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_location_guid",
                schema: "core",
                table: "Roles",
                column: "location_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_locationid",
                schema: "core",
                table: "Companies",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorLocation_location_guid",
                schema: "core",
                table: "OperatorLocation",
                column: "location_guid");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorLocation_operator_guid",
                schema: "core",
                table: "OperatorLocation",
                column: "operator_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_role_guid",
                schema: "core",
                table: "Operators",
                column: "role_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Locations_locationid",
                schema: "core",
                table: "Companies",
                column: "locationid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Locations_location_guid",
                schema: "core",
                table: "Roles",
                column: "location_guid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Locations_location_guid",
                schema: "core",
                table: "Users",
                column: "location_guid",
                principalSchema: "core",
                principalTable: "Locations",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_roleid",
                schema: "core",
                table: "Users",
                column: "roleid",
                principalSchema: "core",
                principalTable: "Roles",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Locations_locationid",
                schema: "core",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Locations_location_guid",
                schema: "core",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Locations_location_guid",
                schema: "core",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_roleid",
                schema: "core",
                table: "Users");

            migrationBuilder.DropTable(
                name: "OperatorLocation",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Operators",
                schema: "core");

            migrationBuilder.DropIndex(
                name: "IX_Users_location_guid",
                schema: "core",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_roleid",
                schema: "core",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Roles_location_guid",
                schema: "core",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Companies_locationid",
                schema: "core",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "location_guid",
                schema: "core",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "roleid",
                schema: "core",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "location_guid",
                schema: "core",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "location_guid",
                schema: "core",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "locationid",
                schema: "core",
                table: "Companies");

            migrationBuilder.UpdateData(
                schema: "core",
                table: "Locations",
                keyColumn: "id",
                keyValue: 1,
                column: "guid",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "core",
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                column: "is_default",
                value: false);

            migrationBuilder.InsertData(
                schema: "core",
                table: "Users",
                columns: new[] { "id", "active_time", "address", "company_guid", "date_of_birth", "department_guid", "email", "expire_time", "face_guid", "firstname", "gender", "identification", "is_active", "is_default", "is_operator", "lastname", "middlename", "password", "phone", "position_guid", "role_guid", "title", "username" },
                values: new object[] { 1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sentrix", null, new DateTime(1996, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, "support@sentrix.com", new DateTime(9999, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Administrator", "M", "Administrator", true, false, true, "", "", "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=", "", null, new Guid("fe527691-7b13-4294-98b5-cb95181f5453"), "Mr.", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_guid",
                schema: "core",
                table: "Users",
                column: "role_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_role_guid",
                schema: "core",
                table: "Users",
                column: "role_guid",
                principalSchema: "core",
                principalTable: "Roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
