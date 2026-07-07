using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Role.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "role");

            migrationBuilder.CreateTable(
                name: "features",
                schema: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_features", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    feature_id = table.Column<int>(type: "integer", nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_created = table.Column<bool>(type: "boolean", nullable: false),
                    is_updated = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_permissions_features_feature_id",
                        column: x => x.feature_id,
                        principalSchema: "role",
                        principalTable: "features",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "role",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_operators",
                schema: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_operators", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_operators_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "role",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "role",
                table: "features",
                columns: new[] { "id", "component_id", "is_active", "is_default", "location_id", "name" },
                values: new object[,]
                {
                    { 1, (short)0, true, false, 0, "dashboard" },
                    { 2, (short)0, true, false, 0, "events" },
                    { 3, (short)0, true, false, 0, "location" },
                    { 4, (short)0, true, false, 0, "alert" },
                    { 5, (short)0, true, false, 0, "operator" },
                    { 6, (short)0, true, false, 0, "device" },
                    { 7, (short)0, true, false, 0, "control" },
                    { 8, (short)0, true, false, 0, "monitor" },
                    { 9, (short)0, true, false, 0, "monitorgroup" },
                    { 10, (short)0, true, false, 0, "acr" },
                    { 11, (short)0, true, false, 0, "user" },
                    { 12, (short)0, true, false, 0, "group" },
                    { 13, (short)0, true, false, 0, "area" },
                    { 14, (short)0, true, false, 0, "time" },
                    { 15, (short)0, true, false, 0, "trigger" },
                    { 16, (short)0, true, false, 0, "map" },
                    { 17, (short)0, true, false, 0, "report" },
                    { 18, (short)0, true, false, 0, "setting" },
                    { 19, (short)0, true, false, 0, "tools" }
                });

            migrationBuilder.InsertData(
                schema: "role",
                table: "roles",
                columns: new[] { "id", "component_id", "is_active", "is_default", "location_id", "name" },
                values: new object[] { 1, (short)0, true, true, 1, "Administrator" });

            migrationBuilder.InsertData(
                schema: "role",
                table: "permissions",
                columns: new[] { "id", "component_id", "feature_id", "is_active", "is_created", "is_default", "is_deleted", "is_enabled", "is_updated", "location_id", "role_id" },
                values: new object[,]
                {
                    { 1, (short)0, 1, true, true, false, true, true, true, 0, 1 },
                    { 2, (short)0, 2, true, true, false, true, true, true, 0, 1 },
                    { 3, (short)0, 3, true, true, false, true, true, true, 0, 1 },
                    { 4, (short)0, 4, true, true, false, true, true, true, 0, 1 },
                    { 5, (short)0, 5, true, true, false, true, true, true, 0, 1 },
                    { 6, (short)0, 6, true, true, false, true, true, true, 0, 1 },
                    { 7, (short)0, 7, true, true, false, true, true, true, 0, 1 },
                    { 8, (short)0, 8, true, true, false, true, true, true, 0, 1 },
                    { 9, (short)0, 9, true, true, false, true, true, true, 0, 1 },
                    { 10, (short)0, 10, true, true, false, true, true, true, 0, 1 },
                    { 11, (short)0, 11, true, true, false, true, true, true, 0, 1 },
                    { 12, (short)0, 12, true, true, false, true, true, true, 0, 1 },
                    { 13, (short)0, 13, true, true, false, true, true, true, 0, 1 },
                    { 14, (short)0, 14, true, true, false, true, true, true, 0, 1 },
                    { 15, (short)0, 15, true, true, false, true, true, true, 0, 1 },
                    { 16, (short)0, 16, true, true, false, true, true, true, 0, 1 },
                    { 17, (short)0, 17, true, true, false, true, true, true, 0, 1 },
                    { 18, (short)0, 18, true, true, false, true, true, true, 0, 1 },
                    { 19, (short)0, 19, true, true, false, true, true, true, 0, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_permissions_feature_id",
                schema: "role",
                table: "permissions",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_role_id",
                schema: "role",
                table: "permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_operators_role_id",
                schema: "role",
                table: "role_operators",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "permissions",
                schema: "role");

            migrationBuilder.DropTable(
                name: "role_operators",
                schema: "role");

            migrationBuilder.DropTable(
                name: "features",
                schema: "role");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "role");
        }
    }
}
