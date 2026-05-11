using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Operator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialOperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "operator");

            migrationBuilder.CreateTable(
                name: "operators",
                schema: "operator",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    middlename = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    mobile = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false),
                    position_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OperatorLocation",
                schema: "operator",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorLocation", x => x.id);
                    table.ForeignKey(
                        name: "FK_OperatorLocation_operators_operator_id",
                        column: x => x.operator_id,
                        principalSchema: "operator",
                        principalTable: "operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "operator",
                table: "operators",
                columns: new[] { "id", "company_id", "department_id", "email", "firstname", "gender", "is_active", "lastname", "middlename", "mobile", "password", "position_id", "role_id", "title", "username" },
                values: new object[] { 1, 0, 0, "admin@sentrix.com", "Administrator", "M", true, "", "", "1234567890", "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=", 0, 1, "Administrator", "admin" });

            migrationBuilder.InsertData(
                schema: "operator",
                table: "OperatorLocation",
                columns: new[] { "id", "is_active", "location_id", "operator_id" },
                values: new object[] { 1, true, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_OperatorLocation_operator_id",
                schema: "operator",
                table: "OperatorLocation",
                column: "operator_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperatorLocation",
                schema: "operator");

            migrationBuilder.DropTable(
                name: "operators",
                schema: "operator");
        }
    }
}
