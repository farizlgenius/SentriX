using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Location.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "location");

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Locations_Countries_country_id",
                        column: x => x.country_id,
                        principalSchema: "location",
                        principalTable: "Countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "location",
                table: "Countries",
                columns: new[] { "id", "code", "component_id", "is_active", "location_id", "name" },
                values: new object[,]
                {
                    { 1, "AD", (short)0, true, 0, "Andorra" },
                    { 2, "AE", (short)0, true, 0, "United Arab Emirates" },
                    { 3, "AF", (short)0, true, 0, "Afghanistan" },
                    { 4, "AG", (short)0, true, 0, "Antigua and Barbuda" },
                    { 5, "AI", (short)0, true, 0, "Anguilla" },
                    { 6, "AL", (short)0, true, 0, "Albania" },
                    { 7, "AM", (short)0, true, 0, "Armenia" },
                    { 8, "AN", (short)0, true, 0, "Netherlands Antilles" },
                    { 9, "AO", (short)0, true, 0, "Angola" },
                    { 10, "AQ", (short)0, true, 0, "Antarctica" },
                    { 11, "AR", (short)0, true, 0, "Argentina" },
                    { 12, "AS", (short)0, true, 0, "American Samoa" },
                    { 13, "AT", (short)0, true, 0, "Austria" },
                    { 14, "AU", (short)0, true, 0, "Australia" },
                    { 15, "AW", (short)0, true, 0, "Aruba" },
                    { 16, "AZ", (short)0, true, 0, "Azerbaijan" },
                    { 17, "BA", (short)0, true, 0, "Bosnia and Herzegovina" },
                    { 18, "BB", (short)0, true, 0, "Barbados" },
                    { 19, "BD", (short)0, true, 0, "Bangladesh" },
                    { 20, "BE", (short)0, true, 0, "Belgium" },
                    { 21, "BF", (short)0, true, 0, "Burkina Faso" },
                    { 22, "BG", (short)0, true, 0, "Bulgaria" },
                    { 23, "BH", (short)0, true, 0, "Bahrain" },
                    { 24, "BI", (short)0, true, 0, "Burundi" },
                    { 25, "BJ", (short)0, true, 0, "Benin" },
                    { 26, "BM", (short)0, true, 0, "Bermuda" },
                    { 27, "BN", (short)0, true, 0, "Brunei" },
                    { 28, "BO", (short)0, true, 0, "Bolivia" },
                    { 29, "BR", (short)0, true, 0, "Brazil" },
                    { 30, "BS", (short)0, true, 0, "Bahamas" },
                    { 31, "BT", (short)0, true, 0, "Bhutan" },
                    { 32, "BV", (short)0, true, 0, "Bouvet Island" },
                    { 33, "BW", (short)0, true, 0, "Botswana" },
                    { 34, "BY", (short)0, true, 0, "Belarus" },
                    { 35, "BZ", (short)0, true, 0, "Belize" },
                    { 36, "CA", (short)0, true, 0, "Canada" },
                    { 37, "CC", (short)0, true, 0, "Cocos (Keeling) Islands" },
                    { 38, "CD", (short)0, true, 0, "Congo (DRC)" },
                    { 39, "CF", (short)0, true, 0, "Central African Republic" },
                    { 40, "CG", (short)0, true, 0, "Congo (Republic)" },
                    { 41, "CH", (short)0, true, 0, "Switzerland" },
                    { 42, "CI", (short)0, true, 0, "Côte d'Ivoire" },
                    { 43, "CK", (short)0, true, 0, "Cook Islands" },
                    { 44, "CL", (short)0, true, 0, "Chile" },
                    { 45, "CM", (short)0, true, 0, "Cameroon" },
                    { 46, "CN", (short)0, true, 0, "China" },
                    { 47, "CO", (short)0, true, 0, "Colombia" },
                    { 48, "CR", (short)0, true, 0, "Costa Rica" },
                    { 49, "CU", (short)0, true, 0, "Cuba" },
                    { 50, "CV", (short)0, true, 0, "Cape Verde" },
                    { 51, "CX", (short)0, true, 0, "Christmas Island" },
                    { 52, "CY", (short)0, true, 0, "Cyprus" },
                    { 53, "CZ", (short)0, true, 0, "Czech Republic" },
                    { 54, "DE", (short)0, true, 0, "Germany" },
                    { 55, "DJ", (short)0, true, 0, "Djibouti" },
                    { 56, "DK", (short)0, true, 0, "Denmark" },
                    { 57, "DM", (short)0, true, 0, "Dominica" },
                    { 58, "DO", (short)0, true, 0, "Dominican Republic" },
                    { 59, "DZ", (short)0, true, 0, "Algeria" },
                    { 60, "EC", (short)0, true, 0, "Ecuador" },
                    { 61, "EE", (short)0, true, 0, "Estonia" },
                    { 62, "EG", (short)0, true, 0, "Egypt" },
                    { 63, "EH", (short)0, true, 0, "Western Sahara" },
                    { 64, "ER", (short)0, true, 0, "Eritrea" },
                    { 65, "ES", (short)0, true, 0, "Spain" },
                    { 66, "ET", (short)0, true, 0, "Ethiopia" },
                    { 67, "FI", (short)0, true, 0, "Finland" },
                    { 68, "FJ", (short)0, true, 0, "Fiji" },
                    { 69, "FK", (short)0, true, 0, "Falkland Islands" },
                    { 70, "FM", (short)0, true, 0, "Micronesia" },
                    { 71, "FO", (short)0, true, 0, "Faroe Islands" },
                    { 72, "FR", (short)0, true, 0, "France" },
                    { 73, "GA", (short)0, true, 0, "Gabon" },
                    { 74, "GB", (short)0, true, 0, "United Kingdom" },
                    { 75, "GD", (short)0, true, 0, "Grenada" },
                    { 76, "GE", (short)0, true, 0, "Georgia" },
                    { 77, "GF", (short)0, true, 0, "French Guiana" },
                    { 78, "GG", (short)0, true, 0, "Guernsey" },
                    { 79, "GH", (short)0, true, 0, "Ghana" },
                    { 80, "GI", (short)0, true, 0, "Gibraltar" },
                    { 81, "GL", (short)0, true, 0, "Greenland" },
                    { 82, "GM", (short)0, true, 0, "Gambia" },
                    { 83, "GN", (short)0, true, 0, "Guinea" },
                    { 84, "GP", (short)0, true, 0, "Guadeloupe" },
                    { 85, "GQ", (short)0, true, 0, "Equatorial Guinea" },
                    { 86, "GR", (short)0, true, 0, "Greece" },
                    { 87, "GT", (short)0, true, 0, "Guatemala" },
                    { 88, "GU", (short)0, true, 0, "Guam" },
                    { 89, "GW", (short)0, true, 0, "Guinea-Bissau" },
                    { 90, "GY", (short)0, true, 0, "Guyana" },
                    { 91, "HK", (short)0, true, 0, "Hong Kong" },
                    { 92, "HN", (short)0, true, 0, "Honduras" },
                    { 93, "HR", (short)0, true, 0, "Croatia" },
                    { 94, "HT", (short)0, true, 0, "Haiti" },
                    { 95, "HU", (short)0, true, 0, "Hungary" },
                    { 96, "ID", (short)0, true, 0, "Indonesia" },
                    { 97, "IE", (short)0, true, 0, "Ireland" },
                    { 98, "IL", (short)0, true, 0, "Israel" },
                    { 99, "IN", (short)0, true, 0, "India" },
                    { 100, "IQ", (short)0, true, 0, "Iraq" },
                    { 101, "IR", (short)0, true, 0, "Iran" },
                    { 102, "IS", (short)0, true, 0, "Iceland" },
                    { 103, "IT", (short)0, true, 0, "Italy" },
                    { 104, "JM", (short)0, true, 0, "Jamaica" },
                    { 105, "JO", (short)0, true, 0, "Jordan" },
                    { 106, "JP", (short)0, true, 0, "Japan" },
                    { 107, "KE", (short)0, true, 0, "Kenya" },
                    { 108, "KH", (short)0, true, 0, "Cambodia" },
                    { 109, "KR", (short)0, true, 0, "South Korea" },
                    { 110, "KW", (short)0, true, 0, "Kuwait" },
                    { 111, "KZ", (short)0, true, 0, "Kazakhstan" },
                    { 112, "LA", (short)0, true, 0, "Laos" },
                    { 113, "LB", (short)0, true, 0, "Lebanon" },
                    { 114, "LK", (short)0, true, 0, "Sri Lanka" },
                    { 115, "LR", (short)0, true, 0, "Liberia" },
                    { 116, "LS", (short)0, true, 0, "Lesotho" },
                    { 117, "LT", (short)0, true, 0, "Lithuania" },
                    { 118, "LU", (short)0, true, 0, "Luxembourg" },
                    { 119, "LV", (short)0, true, 0, "Latvia" },
                    { 120, "LY", (short)0, true, 0, "Libya" },
                    { 121, "MA", (short)0, true, 0, "Morocco" },
                    { 122, "MC", (short)0, true, 0, "Monaco" },
                    { 123, "MD", (short)0, true, 0, "Moldova" },
                    { 124, "ME", (short)0, true, 0, "Montenegro" },
                    { 125, "MG", (short)0, true, 0, "Madagascar" },
                    { 126, "MV", (short)0, true, 0, "Maldives" },
                    { 127, "MX", (short)0, true, 0, "Mexico" },
                    { 128, "MY", (short)0, true, 0, "Malaysia" },
                    { 129, "MZ", (short)0, true, 0, "Mozambique" },
                    { 130, "NA", (short)0, true, 0, "Namibia" },
                    { 131, "NG", (short)0, true, 0, "Nigeria" },
                    { 132, "NL", (short)0, true, 0, "Netherlands" },
                    { 133, "NO", (short)0, true, 0, "Norway" },
                    { 134, "NP", (short)0, true, 0, "Nepal" },
                    { 135, "NZ", (short)0, true, 0, "New Zealand" },
                    { 136, "OM", (short)0, true, 0, "Oman" },
                    { 137, "PA", (short)0, true, 0, "Panama" },
                    { 138, "PE", (short)0, true, 0, "Peru" },
                    { 139, "PH", (short)0, true, 0, "Philippines" },
                    { 140, "PK", (short)0, true, 0, "Pakistan" },
                    { 141, "PL", (short)0, true, 0, "Poland" },
                    { 142, "PT", (short)0, true, 0, "Portugal" },
                    { 143, "QA", (short)0, true, 0, "Qatar" },
                    { 144, "RO", (short)0, true, 0, "Romania" },
                    { 145, "RS", (short)0, true, 0, "Serbia" },
                    { 146, "RU", (short)0, true, 0, "Russia" },
                    { 147, "RW", (short)0, true, 0, "Rwanda" },
                    { 148, "SA", (short)0, true, 0, "Saudi Arabia" },
                    { 149, "SE", (short)0, true, 0, "Sweden" },
                    { 150, "SG", (short)0, true, 0, "Singapore" },
                    { 151, "SI", (short)0, true, 0, "Slovenia" },
                    { 152, "SK", (short)0, true, 0, "Slovakia" },
                    { 153, "SN", (short)0, true, 0, "Senegal" },
                    { 154, "SO", (short)0, true, 0, "Somalia" },
                    { 155, "SR", (short)0, true, 0, "Suriname" },
                    { 156, "SV", (short)0, true, 0, "El Salvador" },
                    { 157, "SY", (short)0, true, 0, "Syria" },
                    { 158, "TH", (short)0, true, 0, "Thailand" },
                    { 159, "TJ", (short)0, true, 0, "Tajikistan" },
                    { 160, "TL", (short)0, true, 0, "Timor-Leste" },
                    { 161, "TM", (short)0, true, 0, "Turkmenistan" },
                    { 162, "TN", (short)0, true, 0, "Tunisia" },
                    { 163, "TR", (short)0, true, 0, "Turkey" },
                    { 164, "TW", (short)0, true, 0, "Taiwan" },
                    { 165, "TZ", (short)0, true, 0, "Tanzania" },
                    { 166, "UA", (short)0, true, 0, "Ukraine" },
                    { 167, "UG", (short)0, true, 0, "Uganda" },
                    { 168, "US", (short)0, true, 0, "United States" },
                    { 169, "UY", (short)0, true, 0, "Uruguay" },
                    { 170, "UZ", (short)0, true, 0, "Uzbekistan" },
                    { 171, "VA", (short)0, true, 0, "Vatican City" },
                    { 172, "VE", (short)0, true, 0, "Venezuela" },
                    { 173, "VN", (short)0, true, 0, "Vietnam" },
                    { 174, "YE", (short)0, true, 0, "Yemen" },
                    { 175, "ZA", (short)0, true, 0, "South Africa" },
                    { 176, "ZM", (short)0, true, 0, "Zambia" },
                    { 177, "ZW", (short)0, true, 0, "Zimbabwe" },
                    { 178, "", (short)0, true, 0, "Default" }
                });

            migrationBuilder.InsertData(
                schema: "location",
                table: "Locations",
                columns: new[] { "id", "component_id", "country_id", "description", "is_active", "location_id", "name" },
                values: new object[] { 1, (short)0, 178, "Main location descriptions", true, 0, "Main Location" });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_country_id",
                schema: "location",
                table: "Locations",
                column: "country_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations",
                schema: "location");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "location");
        }
    }
}
