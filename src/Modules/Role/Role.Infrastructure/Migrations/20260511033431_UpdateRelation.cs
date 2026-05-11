using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Role.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelation : Migration
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
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
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
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
                columns: new[] { "id", "is_active", "name" },
                values: new object[,]
                {
                    { 1, true, "dashboard" },
                    { 2, true, "events" },
                    { 3, true, "location" },
                    { 4, true, "alert" },
                    { 5, true, "operator" },
                    { 6, true, "device" },
                    { 7, true, "control" },
                    { 8, true, "monitor" },
                    { 9, true, "monitorgroup" },
                    { 10, true, "acr" },
                    { 11, true, "user" },
                    { 12, true, "group" },
                    { 13, true, "area" },
                    { 14, true, "time" },
                    { 15, true, "trigger" },
                    { 16, true, "map" },
                    { 17, true, "report" },
                    { 18, true, "setting" },
                    { 19, true, "tools" }
                });

            migrationBuilder.InsertData(
                schema: "role",
                table: "roles",
                columns: new[] { "id", "is_active", "location_id", "name" },
                values: new object[] { 1, true, 1, "Administrator" });

            migrationBuilder.InsertData(
                schema: "role",
                table: "permissions",
                columns: new[] { "id", "feature_id", "is_active", "is_created", "is_deleted", "is_enabled", "is_updated", "role_id" },
                values: new object[,]
                {
                    { 1, 1, true, true, true, true, true, 1 },
                    { 2, 2, true, true, true, true, true, 1 },
                    { 3, 3, true, true, true, true, true, 1 },
                    { 4, 4, true, true, true, true, true, 1 },
                    { 5, 5, true, true, true, true, true, 1 },
                    { 6, 6, true, true, true, true, true, 1 },
                    { 7, 7, true, true, true, true, true, 1 },
                    { 8, 8, true, true, true, true, true, 1 },
                    { 9, 9, true, true, true, true, true, 1 },
                    { 10, 10, true, true, true, true, true, 1 },
                    { 11, 11, true, true, true, true, true, 1 },
                    { 12, 12, true, true, true, true, true, 1 },
                    { 13, 13, true, true, true, true, true, 1 },
                    { 14, 14, true, true, true, true, true, 1 },
                    { 15, 15, true, true, true, true, true, 1 },
                    { 16, 16, true, true, true, true, true, 1 },
                    { 17, 17, true, true, true, true, true, 1 },
                    { 18, 18, true, true, true, true, true, 1 },
                    { 19, 19, true, true, true, true, true, 1 }
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
