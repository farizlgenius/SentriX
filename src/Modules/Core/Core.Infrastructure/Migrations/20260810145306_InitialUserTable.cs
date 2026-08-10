using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.id);
                    table.UniqueConstraint("AK_Companies_guid", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Faces",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image_name = table.Column<string>(type: "text", nullable: false),
                    user_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faces", x => x.id);
                    table.UniqueConstraint("AK_Faces_guid", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.id);
                    table.UniqueConstraint("AK_Features_guid", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                    table.UniqueConstraint("AK_Roles_guid", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    company_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.id);
                    table.UniqueConstraint("AK_Departments_guid", x => x.guid);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_company_guid",
                        column: x => x.company_guid,
                        principalSchema: "core",
                        principalTable: "Companies",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.id);
                    table.UniqueConstraint("AK_Locations_guid", x => x.guid);
                    table.ForeignKey(
                        name: "FK_Locations_Countries_country_id",
                        column: x => x.country_id,
                        principalSchema: "core",
                        principalTable: "Countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    feature_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_created = table.Column<bool>(type: "boolean", nullable: false),
                    is_updated = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Permissions_Features_feature_guid",
                        column: x => x.feature_guid,
                        principalSchema: "core",
                        principalTable: "Features",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_role_guid",
                        column: x => x.role_guid,
                        principalSchema: "core",
                        principalTable: "Roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    department_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.id);
                    table.UniqueConstraint("AK_Positions_guid", x => x.guid);
                    table.ForeignKey(
                        name: "FK_Positions_Departments_department_guid",
                        column: x => x.department_guid,
                        principalSchema: "core",
                        principalTable: "Departments",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    fw = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false),
                    location_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.id);
                    table.UniqueConstraint("AK_Devices_guid", x => x.guid);
                    table.ForeignKey(
                        name: "FK_Devices_Locations_location_guid",
                        column: x => x.location_guid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    identification = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    middlename = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    is_operator = table.Column<bool>(type: "boolean", nullable: false),
                    role_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    company_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    department_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    position_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    address = table.Column<string>(type: "text", nullable: false),
                    active_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    face_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.UniqueConstraint("AK_Users_guid", x => x.guid);
                    table.ForeignKey(
                        name: "FK_Users_Companies_company_guid",
                        column: x => x.company_guid,
                        principalSchema: "core",
                        principalTable: "Companies",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Departments_department_guid",
                        column: x => x.department_guid,
                        principalSchema: "core",
                        principalTable: "Departments",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Faces_face_guid",
                        column: x => x.face_guid,
                        principalSchema: "core",
                        principalTable: "Faces",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Positions_position_guid",
                        column: x => x.position_guid,
                        principalSchema: "core",
                        principalTable: "Positions",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_role_guid",
                        column: x => x.role_guid,
                        principalSchema: "core",
                        principalTable: "Roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    fw = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<short>(type: "smallint", nullable: false),
                    address = table.Column<short>(type: "smallint", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    device_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    location_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.id);
                    table.ForeignKey(
                        name: "FK_Modules_Devices_device_guid",
                        column: x => x.device_guid,
                        principalSchema: "core",
                        principalTable: "Devices",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modules_Locations_location_guid",
                        column: x => x.location_guid,
                        principalSchema: "core",
                        principalTable: "Locations",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bits = table.Column<short>(type: "smallint", nullable: false),
                    fac = table.Column<int>(type: "integer", nullable: false),
                    card_number = table.Column<int>(type: "integer", nullable: false),
                    user_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cards_Users_user_guid",
                        column: x => x.user_guid,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicensePlates",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    license_plate = table.Column<string>(type: "text", nullable: false),
                    user_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicensePlates", x => x.id);
                    table.ForeignKey(
                        name: "FK_LicensePlates_Users_user_guid",
                        column: x => x.user_guid,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pins",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pin = table.Column<string>(type: "text", nullable: false),
                    user_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pins", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pins_Users_user_guid",
                        column: x => x.user_guid,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QrCodes",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qr_code = table.Column<string>(type: "text", nullable: false),
                    user_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrCodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_QrCodes_Users_user_guid",
                        column: x => x.user_guid,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionals",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    additional = table.Column<string>(type: "text", nullable: false),
                    user_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdditionals", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserAdditionals_Users_user_guid",
                        column: x => x.user_guid,
                        principalSchema: "core",
                        principalTable: "Users",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Countries",
                columns: new[] { "id", "code", "is_active", "is_default", "name" },
                values: new object[,]
                {
                    { 1, "AD", true, false, "Andorra" },
                    { 2, "AE", true, false, "United Arab Emirates" },
                    { 3, "AF", true, false, "Afghanistan" },
                    { 4, "AG", true, false, "Antigua and Barbuda" },
                    { 5, "AI", true, false, "Anguilla" },
                    { 6, "AL", true, false, "Albania" },
                    { 7, "AM", true, false, "Armenia" },
                    { 8, "AN", true, false, "Netherlands Antilles" },
                    { 9, "AO", true, false, "Angola" },
                    { 10, "AQ", true, false, "Antarctica" },
                    { 11, "AR", true, false, "Argentina" },
                    { 12, "AS", true, false, "American Samoa" },
                    { 13, "AT", true, false, "Austria" },
                    { 14, "AU", true, false, "Australia" },
                    { 15, "AW", true, false, "Aruba" },
                    { 16, "AZ", true, false, "Azerbaijan" },
                    { 17, "BA", true, false, "Bosnia and Herzegovina" },
                    { 18, "BB", true, false, "Barbados" },
                    { 19, "BD", true, false, "Bangladesh" },
                    { 20, "BE", true, false, "Belgium" },
                    { 21, "BF", true, false, "Burkina Faso" },
                    { 22, "BG", true, false, "Bulgaria" },
                    { 23, "BH", true, false, "Bahrain" },
                    { 24, "BI", true, false, "Burundi" },
                    { 25, "BJ", true, false, "Benin" },
                    { 26, "BM", true, false, "Bermuda" },
                    { 27, "BN", true, false, "Brunei" },
                    { 28, "BO", true, false, "Bolivia" },
                    { 29, "BR", true, false, "Brazil" },
                    { 30, "BS", true, false, "Bahamas" },
                    { 31, "BT", true, false, "Bhutan" },
                    { 32, "BV", true, false, "Bouvet Island" },
                    { 33, "BW", true, false, "Botswana" },
                    { 34, "BY", true, false, "Belarus" },
                    { 35, "BZ", true, false, "Belize" },
                    { 36, "CA", true, false, "Canada" },
                    { 37, "CC", true, false, "Cocos (Keeling) Islands" },
                    { 38, "CD", true, false, "Congo (DRC)" },
                    { 39, "CF", true, false, "Central African Republic" },
                    { 40, "CG", true, false, "Congo (Republic)" },
                    { 41, "CH", true, false, "Switzerland" },
                    { 42, "CI", true, false, "Côte d'Ivoire" },
                    { 43, "CK", true, false, "Cook Islands" },
                    { 44, "CL", true, false, "Chile" },
                    { 45, "CM", true, false, "Cameroon" },
                    { 46, "CN", true, false, "China" },
                    { 47, "CO", true, false, "Colombia" },
                    { 48, "CR", true, false, "Costa Rica" },
                    { 49, "CU", true, false, "Cuba" },
                    { 50, "CV", true, false, "Cape Verde" },
                    { 51, "CX", true, false, "Christmas Island" },
                    { 52, "CY", true, false, "Cyprus" },
                    { 53, "CZ", true, false, "Czech Republic" },
                    { 54, "DE", true, false, "Germany" },
                    { 55, "DJ", true, false, "Djibouti" },
                    { 56, "DK", true, false, "Denmark" },
                    { 57, "DM", true, false, "Dominica" },
                    { 58, "DO", true, false, "Dominican Republic" },
                    { 59, "DZ", true, false, "Algeria" },
                    { 60, "EC", true, false, "Ecuador" },
                    { 61, "EE", true, false, "Estonia" },
                    { 62, "EG", true, false, "Egypt" },
                    { 63, "EH", true, false, "Western Sahara" },
                    { 64, "ER", true, false, "Eritrea" },
                    { 65, "ES", true, false, "Spain" },
                    { 66, "ET", true, false, "Ethiopia" },
                    { 67, "FI", true, false, "Finland" },
                    { 68, "FJ", true, false, "Fiji" },
                    { 69, "FK", true, false, "Falkland Islands" },
                    { 70, "FM", true, false, "Micronesia" },
                    { 71, "FO", true, false, "Faroe Islands" },
                    { 72, "FR", true, false, "France" },
                    { 73, "GA", true, false, "Gabon" },
                    { 74, "GB", true, false, "United Kingdom" },
                    { 75, "GD", true, false, "Grenada" },
                    { 76, "GE", true, false, "Georgia" },
                    { 77, "GF", true, false, "French Guiana" },
                    { 78, "GG", true, false, "Guernsey" },
                    { 79, "GH", true, false, "Ghana" },
                    { 80, "GI", true, false, "Gibraltar" },
                    { 81, "GL", true, false, "Greenland" },
                    { 82, "GM", true, false, "Gambia" },
                    { 83, "GN", true, false, "Guinea" },
                    { 84, "GP", true, false, "Guadeloupe" },
                    { 85, "GQ", true, false, "Equatorial Guinea" },
                    { 86, "GR", true, false, "Greece" },
                    { 87, "GT", true, false, "Guatemala" },
                    { 88, "GU", true, false, "Guam" },
                    { 89, "GW", true, false, "Guinea-Bissau" },
                    { 90, "GY", true, false, "Guyana" },
                    { 91, "HK", true, false, "Hong Kong" },
                    { 92, "HN", true, false, "Honduras" },
                    { 93, "HR", true, false, "Croatia" },
                    { 94, "HT", true, false, "Haiti" },
                    { 95, "HU", true, false, "Hungary" },
                    { 96, "ID", true, false, "Indonesia" },
                    { 97, "IE", true, false, "Ireland" },
                    { 98, "IL", true, false, "Israel" },
                    { 99, "IN", true, false, "India" },
                    { 100, "IQ", true, false, "Iraq" },
                    { 101, "IR", true, false, "Iran" },
                    { 102, "IS", true, false, "Iceland" },
                    { 103, "IT", true, false, "Italy" },
                    { 104, "JM", true, false, "Jamaica" },
                    { 105, "JO", true, false, "Jordan" },
                    { 106, "JP", true, false, "Japan" },
                    { 107, "KE", true, false, "Kenya" },
                    { 108, "KH", true, false, "Cambodia" },
                    { 109, "KR", true, false, "South Korea" },
                    { 110, "KW", true, false, "Kuwait" },
                    { 111, "KZ", true, false, "Kazakhstan" },
                    { 112, "LA", true, false, "Laos" },
                    { 113, "LB", true, false, "Lebanon" },
                    { 114, "LK", true, false, "Sri Lanka" },
                    { 115, "LR", true, false, "Liberia" },
                    { 116, "LS", true, false, "Lesotho" },
                    { 117, "LT", true, false, "Lithuania" },
                    { 118, "LU", true, false, "Luxembourg" },
                    { 119, "LV", true, false, "Latvia" },
                    { 120, "LY", true, false, "Libya" },
                    { 121, "MA", true, false, "Morocco" },
                    { 122, "MC", true, false, "Monaco" },
                    { 123, "MD", true, false, "Moldova" },
                    { 124, "ME", true, false, "Montenegro" },
                    { 125, "MG", true, false, "Madagascar" },
                    { 126, "MV", true, false, "Maldives" },
                    { 127, "MX", true, false, "Mexico" },
                    { 128, "MY", true, false, "Malaysia" },
                    { 129, "MZ", true, false, "Mozambique" },
                    { 130, "NA", true, false, "Namibia" },
                    { 131, "NG", true, false, "Nigeria" },
                    { 132, "NL", true, false, "Netherlands" },
                    { 133, "NO", true, false, "Norway" },
                    { 134, "NP", true, false, "Nepal" },
                    { 135, "NZ", true, false, "New Zealand" },
                    { 136, "OM", true, false, "Oman" },
                    { 137, "PA", true, false, "Panama" },
                    { 138, "PE", true, false, "Peru" },
                    { 139, "PH", true, false, "Philippines" },
                    { 140, "PK", true, false, "Pakistan" },
                    { 141, "PL", true, false, "Poland" },
                    { 142, "PT", true, false, "Portugal" },
                    { 143, "QA", true, false, "Qatar" },
                    { 144, "RO", true, false, "Romania" },
                    { 145, "RS", true, false, "Serbia" },
                    { 146, "RU", true, false, "Russia" },
                    { 147, "RW", true, false, "Rwanda" },
                    { 148, "SA", true, false, "Saudi Arabia" },
                    { 149, "SE", true, false, "Sweden" },
                    { 150, "SG", true, false, "Singapore" },
                    { 151, "SI", true, false, "Slovenia" },
                    { 152, "SK", true, false, "Slovakia" },
                    { 153, "SN", true, false, "Senegal" },
                    { 154, "SO", true, false, "Somalia" },
                    { 155, "SR", true, false, "Suriname" },
                    { 156, "SV", true, false, "El Salvador" },
                    { 157, "SY", true, false, "Syria" },
                    { 158, "TH", true, false, "Thailand" },
                    { 159, "TJ", true, false, "Tajikistan" },
                    { 160, "TL", true, false, "Timor-Leste" },
                    { 161, "TM", true, false, "Turkmenistan" },
                    { 162, "TN", true, false, "Tunisia" },
                    { 163, "TR", true, false, "Turkey" },
                    { 164, "TW", true, false, "Taiwan" },
                    { 165, "TZ", true, false, "Tanzania" },
                    { 166, "UA", true, false, "Ukraine" },
                    { 167, "UG", true, false, "Uganda" },
                    { 168, "US", true, false, "United States" },
                    { 169, "UY", true, false, "Uruguay" },
                    { 170, "UZ", true, false, "Uzbekistan" },
                    { 171, "VA", true, false, "Vatican City" },
                    { 172, "VE", true, false, "Venezuela" },
                    { 173, "VN", true, false, "Vietnam" },
                    { 174, "YE", true, false, "Yemen" },
                    { 175, "ZA", true, false, "South Africa" },
                    { 176, "ZM", true, false, "Zambia" },
                    { 177, "ZW", true, false, "Zimbabwe" },
                    { 178, "", true, false, "Default" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Features",
                columns: new[] { "id", "guid", "is_active", "is_default", "name" },
                values: new object[,]
                {
                    { 1, new Guid("f1f1f528-1025-44de-8512-be5f269417e8"), true, false, "dashboard" },
                    { 2, new Guid("62e7ede3-9152-476a-a4df-173cc16a12fe"), true, false, "events" },
                    { 3, new Guid("c164d952-6649-49bb-95c9-2543695b8af6"), true, false, "location" },
                    { 4, new Guid("14fa8dca-521d-4e1a-a582-0159df91aea9"), true, false, "alert" },
                    { 5, new Guid("60239ccd-4cd7-441a-94c4-4a1577c79e38"), true, false, "operator" },
                    { 6, new Guid("dc76438d-0e0d-4d60-88bc-0559cb81ce4a"), true, false, "device" },
                    { 7, new Guid("77c0545d-ec94-4037-802f-2240bcc9020e"), true, false, "control" },
                    { 8, new Guid("2242b3c0-06e7-4e07-be9f-7491584c57c9"), true, false, "monitor" },
                    { 9, new Guid("8b5c31bb-706b-4fa5-b0f0-dd246f1e9a2b"), true, false, "monitorgroup" },
                    { 10, new Guid("f2143a86-d2f1-47ad-a481-c74ecbdadc83"), true, false, "acr" },
                    { 11, new Guid("b753863e-30f4-47aa-81b3-64dda55970da"), true, false, "user" },
                    { 12, new Guid("5ab363b5-a921-41e5-949e-5129eb416097"), true, false, "group" },
                    { 13, new Guid("4401f4d8-4145-4439-adab-d89cd3e3b2fb"), true, false, "area" },
                    { 14, new Guid("f18ef407-cd4d-46e8-a9f5-cbc99b87a0e4"), true, false, "time" },
                    { 15, new Guid("5699f80a-aa8c-4325-88cf-8ba31b85f976"), true, false, "trigger" },
                    { 16, new Guid("76d4a40a-3fa8-4a9f-a7b5-2b63f57fd26d"), true, false, "map" },
                    { 17, new Guid("24ebed55-6686-45a8-95a5-00e4e6516f4f"), true, false, "report" },
                    { 18, new Guid("713bcbae-1755-4a94-ab82-0180a856c80a"), true, false, "setting" },
                    { 19, new Guid("d57aac0d-1f61-4135-857b-cc1f51288d72"), true, false, "tools" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Roles",
                columns: new[] { "id", "guid", "is_active", "is_default", "name" },
                values: new object[] { 1, new Guid("fe527691-7b13-4294-98b5-cb95181f5453"), true, false, "Administrator" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Locations",
                columns: new[] { "id", "country_id", "description", "is_active", "is_default", "name" },
                values: new object[] { 1, 178, "Main location", true, true, "Main Location" });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Permissions",
                columns: new[] { "id", "feature_guid", "is_active", "is_created", "is_default", "is_deleted", "is_enabled", "is_updated", "role_guid" },
                values: new object[,]
                {
                    { 1, new Guid("f1f1f528-1025-44de-8512-be5f269417e8"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 2, new Guid("62e7ede3-9152-476a-a4df-173cc16a12fe"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 3, new Guid("c164d952-6649-49bb-95c9-2543695b8af6"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 4, new Guid("14fa8dca-521d-4e1a-a582-0159df91aea9"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 5, new Guid("60239ccd-4cd7-441a-94c4-4a1577c79e38"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 6, new Guid("dc76438d-0e0d-4d60-88bc-0559cb81ce4a"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 7, new Guid("77c0545d-ec94-4037-802f-2240bcc9020e"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 8, new Guid("2242b3c0-06e7-4e07-be9f-7491584c57c9"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 9, new Guid("8b5c31bb-706b-4fa5-b0f0-dd246f1e9a2b"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 10, new Guid("f2143a86-d2f1-47ad-a481-c74ecbdadc83"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 11, new Guid("b753863e-30f4-47aa-81b3-64dda55970da"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 12, new Guid("5ab363b5-a921-41e5-949e-5129eb416097"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 13, new Guid("4401f4d8-4145-4439-adab-d89cd3e3b2fb"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 14, new Guid("f18ef407-cd4d-46e8-a9f5-cbc99b87a0e4"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 15, new Guid("5699f80a-aa8c-4325-88cf-8ba31b85f976"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 16, new Guid("76d4a40a-3fa8-4a9f-a7b5-2b63f57fd26d"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 17, new Guid("24ebed55-6686-45a8-95a5-00e4e6516f4f"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 18, new Guid("713bcbae-1755-4a94-ab82-0180a856c80a"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") },
                    { 19, new Guid("d57aac0d-1f61-4135-857b-cc1f51288d72"), true, true, false, true, true, true, new Guid("fe527691-7b13-4294-98b5-cb95181f5453") }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Users",
                columns: new[] { "id", "active_time", "address", "company_guid", "date_of_birth", "department_guid", "email", "expire_time", "face_guid", "firstname", "gender", "identification", "is_active", "is_default", "is_operator", "lastname", "middlename", "password", "phone", "position_guid", "role_guid", "title", "username" },
                values: new object[] { 1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sentrix", null, new DateTime(1996, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, "support@sentrix.com", new DateTime(9999, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Administrator", "M", "Administrator", true, false, true, "", "", "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=", "", null, new Guid("fe527691-7b13-4294-98b5-cb95181f5453"), "Mr.", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_user_guid",
                schema: "core",
                table: "Cards",
                column: "user_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_company_guid",
                schema: "core",
                table: "Departments",
                column: "company_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_location_guid",
                schema: "core",
                table: "Devices",
                column: "location_guid");

            migrationBuilder.CreateIndex(
                name: "IX_LicensePlates_user_guid",
                schema: "core",
                table: "LicensePlates",
                column: "user_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_country_id",
                schema: "core",
                table: "Locations",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_device_guid",
                schema: "core",
                table: "Modules",
                column: "device_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_location_guid",
                schema: "core",
                table: "Modules",
                column: "location_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_feature_guid",
                schema: "core",
                table: "Permissions",
                column: "feature_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_role_guid",
                schema: "core",
                table: "Permissions",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_user_guid",
                schema: "core",
                table: "Pins",
                column: "user_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_department_guid",
                schema: "core",
                table: "Positions",
                column: "department_guid");

            migrationBuilder.CreateIndex(
                name: "IX_QrCodes_user_guid",
                schema: "core",
                table: "QrCodes",
                column: "user_guid");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionals_user_guid",
                schema: "core",
                table: "UserAdditionals",
                column: "user_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_company_guid",
                schema: "core",
                table: "Users",
                column: "company_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_department_guid",
                schema: "core",
                table: "Users",
                column: "department_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_face_guid",
                schema: "core",
                table: "Users",
                column: "face_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_position_guid",
                schema: "core",
                table: "Users",
                column: "position_guid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_guid",
                schema: "core",
                table: "Users",
                column: "role_guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards",
                schema: "core");

            migrationBuilder.DropTable(
                name: "LicensePlates",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Pins",
                schema: "core");

            migrationBuilder.DropTable(
                name: "QrCodes",
                schema: "core");

            migrationBuilder.DropTable(
                name: "UserAdditionals",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Features",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Faces",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "core");
        }
    }
}
