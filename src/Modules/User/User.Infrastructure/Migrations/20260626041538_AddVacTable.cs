using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVacTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "vacation_id",
                schema: "user",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "apb_loc",
                schema: "user",
                table: "Credentials",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "fac",
                schema: "user",
                table: "Credentials",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "Vacation",
                schema: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vacation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vacation_day = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacation", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_vacation_id",
                schema: "user",
                table: "Users",
                column: "vacation_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Vacation_vacation_id",
                schema: "user",
                table: "Users",
                column: "vacation_id",
                principalSchema: "user",
                principalTable: "Vacation",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Vacation_vacation_id",
                schema: "user",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Vacation",
                schema: "user");

            migrationBuilder.DropIndex(
                name: "IX_Users_vacation_id",
                schema: "user",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "vacation_id",
                schema: "user",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "apb_loc",
                schema: "user",
                table: "Credentials");

            migrationBuilder.DropColumn(
                name: "fac",
                schema: "user",
                table: "Credentials");
        }
    }
}
