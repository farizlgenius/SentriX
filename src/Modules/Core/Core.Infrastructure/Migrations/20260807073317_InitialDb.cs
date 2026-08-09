using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.CreateSequence(
                name: "BaseEntitySequence",
                schema: "core");

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    country_id = table.Column<int>(type: "integer", nullable: false),
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
                name: "Devices",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('core.\"BaseEntitySequence\"')"),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    location_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    fw = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false)
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
                name: "Modules",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('core.\"BaseEntitySequence\"')"),
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    vendor = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    location_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    fw = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<short>(type: "smallint", nullable: false),
                    address = table.Column<short>(type: "smallint", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    device_guid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.id);
                    table.UniqueConstraint("AK_Modules_guid", x => x.guid);
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

            migrationBuilder.InsertData(
                schema: "core",
                table: "Countries",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { 1, "AD", "Andorra" },
                    { 2, "AE", "United Arab Emirates" },
                    { 3, "AF", "Afghanistan" },
                    { 4, "AG", "Antigua and Barbuda" },
                    { 5, "AI", "Anguilla" },
                    { 6, "AL", "Albania" },
                    { 7, "AM", "Armenia" },
                    { 8, "AN", "Netherlands Antilles" },
                    { 9, "AO", "Angola" },
                    { 10, "AQ", "Antarctica" },
                    { 11, "AR", "Argentina" },
                    { 12, "AS", "American Samoa" },
                    { 13, "AT", "Austria" },
                    { 14, "AU", "Australia" },
                    { 15, "AW", "Aruba" },
                    { 16, "AZ", "Azerbaijan" },
                    { 17, "BA", "Bosnia and Herzegovina" },
                    { 18, "BB", "Barbados" },
                    { 19, "BD", "Bangladesh" },
                    { 20, "BE", "Belgium" },
                    { 21, "BF", "Burkina Faso" },
                    { 22, "BG", "Bulgaria" },
                    { 23, "BH", "Bahrain" },
                    { 24, "BI", "Burundi" },
                    { 25, "BJ", "Benin" },
                    { 26, "BM", "Bermuda" },
                    { 27, "BN", "Brunei" },
                    { 28, "BO", "Bolivia" },
                    { 29, "BR", "Brazil" },
                    { 30, "BS", "Bahamas" },
                    { 31, "BT", "Bhutan" },
                    { 32, "BV", "Bouvet Island" },
                    { 33, "BW", "Botswana" },
                    { 34, "BY", "Belarus" },
                    { 35, "BZ", "Belize" },
                    { 36, "CA", "Canada" },
                    { 37, "CC", "Cocos (Keeling) Islands" },
                    { 38, "CD", "Congo (DRC)" },
                    { 39, "CF", "Central African Republic" },
                    { 40, "CG", "Congo (Republic)" },
                    { 41, "CH", "Switzerland" },
                    { 42, "CI", "Côte d'Ivoire" },
                    { 43, "CK", "Cook Islands" },
                    { 44, "CL", "Chile" },
                    { 45, "CM", "Cameroon" },
                    { 46, "CN", "China" },
                    { 47, "CO", "Colombia" },
                    { 48, "CR", "Costa Rica" },
                    { 49, "CU", "Cuba" },
                    { 50, "CV", "Cape Verde" },
                    { 51, "CX", "Christmas Island" },
                    { 52, "CY", "Cyprus" },
                    { 53, "CZ", "Czech Republic" },
                    { 54, "DE", "Germany" },
                    { 55, "DJ", "Djibouti" },
                    { 56, "DK", "Denmark" },
                    { 57, "DM", "Dominica" },
                    { 58, "DO", "Dominican Republic" },
                    { 59, "DZ", "Algeria" },
                    { 60, "EC", "Ecuador" },
                    { 61, "EE", "Estonia" },
                    { 62, "EG", "Egypt" },
                    { 63, "EH", "Western Sahara" },
                    { 64, "ER", "Eritrea" },
                    { 65, "ES", "Spain" },
                    { 66, "ET", "Ethiopia" },
                    { 67, "FI", "Finland" },
                    { 68, "FJ", "Fiji" },
                    { 69, "FK", "Falkland Islands" },
                    { 70, "FM", "Micronesia" },
                    { 71, "FO", "Faroe Islands" },
                    { 72, "FR", "France" },
                    { 73, "GA", "Gabon" },
                    { 74, "GB", "United Kingdom" },
                    { 75, "GD", "Grenada" },
                    { 76, "GE", "Georgia" },
                    { 77, "GF", "French Guiana" },
                    { 78, "GG", "Guernsey" },
                    { 79, "GH", "Ghana" },
                    { 80, "GI", "Gibraltar" },
                    { 81, "GL", "Greenland" },
                    { 82, "GM", "Gambia" },
                    { 83, "GN", "Guinea" },
                    { 84, "GP", "Guadeloupe" },
                    { 85, "GQ", "Equatorial Guinea" },
                    { 86, "GR", "Greece" },
                    { 87, "GT", "Guatemala" },
                    { 88, "GU", "Guam" },
                    { 89, "GW", "Guinea-Bissau" },
                    { 90, "GY", "Guyana" },
                    { 91, "HK", "Hong Kong" },
                    { 92, "HN", "Honduras" },
                    { 93, "HR", "Croatia" },
                    { 94, "HT", "Haiti" },
                    { 95, "HU", "Hungary" },
                    { 96, "ID", "Indonesia" },
                    { 97, "IE", "Ireland" },
                    { 98, "IL", "Israel" },
                    { 99, "IN", "India" },
                    { 100, "IQ", "Iraq" },
                    { 101, "IR", "Iran" },
                    { 102, "IS", "Iceland" },
                    { 103, "IT", "Italy" },
                    { 104, "JM", "Jamaica" },
                    { 105, "JO", "Jordan" },
                    { 106, "JP", "Japan" },
                    { 107, "KE", "Kenya" },
                    { 108, "KH", "Cambodia" },
                    { 109, "KR", "South Korea" },
                    { 110, "KW", "Kuwait" },
                    { 111, "KZ", "Kazakhstan" },
                    { 112, "LA", "Laos" },
                    { 113, "LB", "Lebanon" },
                    { 114, "LK", "Sri Lanka" },
                    { 115, "LR", "Liberia" },
                    { 116, "LS", "Lesotho" },
                    { 117, "LT", "Lithuania" },
                    { 118, "LU", "Luxembourg" },
                    { 119, "LV", "Latvia" },
                    { 120, "LY", "Libya" },
                    { 121, "MA", "Morocco" },
                    { 122, "MC", "Monaco" },
                    { 123, "MD", "Moldova" },
                    { 124, "ME", "Montenegro" },
                    { 125, "MG", "Madagascar" },
                    { 126, "MV", "Maldives" },
                    { 127, "MX", "Mexico" },
                    { 128, "MY", "Malaysia" },
                    { 129, "MZ", "Mozambique" },
                    { 130, "NA", "Namibia" },
                    { 131, "NG", "Nigeria" },
                    { 132, "NL", "Netherlands" },
                    { 133, "NO", "Norway" },
                    { 134, "NP", "Nepal" },
                    { 135, "NZ", "New Zealand" },
                    { 136, "OM", "Oman" },
                    { 137, "PA", "Panama" },
                    { 138, "PE", "Peru" },
                    { 139, "PH", "Philippines" },
                    { 140, "PK", "Pakistan" },
                    { 141, "PL", "Poland" },
                    { 142, "PT", "Portugal" },
                    { 143, "QA", "Qatar" },
                    { 144, "RO", "Romania" },
                    { 145, "RS", "Serbia" },
                    { 146, "RU", "Russia" },
                    { 147, "RW", "Rwanda" },
                    { 148, "SA", "Saudi Arabia" },
                    { 149, "SE", "Sweden" },
                    { 150, "SG", "Singapore" },
                    { 151, "SI", "Slovenia" },
                    { 152, "SK", "Slovakia" },
                    { 153, "SN", "Senegal" },
                    { 154, "SO", "Somalia" },
                    { 155, "SR", "Suriname" },
                    { 156, "SV", "El Salvador" },
                    { 157, "SY", "Syria" },
                    { 158, "TH", "Thailand" },
                    { 159, "TJ", "Tajikistan" },
                    { 160, "TL", "Timor-Leste" },
                    { 161, "TM", "Turkmenistan" },
                    { 162, "TN", "Tunisia" },
                    { 163, "TR", "Turkey" },
                    { 164, "TW", "Taiwan" },
                    { 165, "TZ", "Tanzania" },
                    { 166, "UA", "Ukraine" },
                    { 167, "UG", "Uganda" },
                    { 168, "US", "United States" },
                    { 169, "UY", "Uruguay" },
                    { 170, "UZ", "Uzbekistan" },
                    { 171, "VA", "Vatican City" },
                    { 172, "VE", "Venezuela" },
                    { 173, "VN", "Vietnam" },
                    { 174, "YE", "Yemen" },
                    { 175, "ZA", "South Africa" },
                    { 176, "ZM", "Zambia" },
                    { 177, "ZW", "Zimbabwe" },
                    { 178, "", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "core",
                table: "Locations",
                columns: new[] { "id", "country_id", "description", "is_active", "is_default", "name" },
                values: new object[] { 1, 178, "Main location descriptions", true, true, "Main Location" });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_location_guid",
                schema: "core",
                table: "Devices",
                column: "location_guid");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modules",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "core");

            migrationBuilder.DropSequence(
                name: "BaseEntitySequence",
                schema: "core");
        }
    }
}
