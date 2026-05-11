using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Location.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialLocation : Migration
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
                columns: new[] { "id", "code", "is_active", "name" },
                values: new object[,]
                {
                    { 1, "AD", true, "Andorra" },
                    { 2, "AE", true, "United Arab Emirates" },
                    { 3, "AF", true, "Afghanistan" },
                    { 4, "AG", true, "Antigua and Barbuda" },
                    { 5, "AI", true, "Anguilla" },
                    { 6, "AL", true, "Albania" },
                    { 7, "AM", true, "Armenia" },
                    { 8, "AN", true, "Netherlands Antilles" },
                    { 9, "AO", true, "Angola" },
                    { 10, "AQ", true, "Antarctica" },
                    { 11, "AR", true, "Argentina" },
                    { 12, "AS", true, "American Samoa" },
                    { 13, "AT", true, "Austria" },
                    { 14, "AU", true, "Australia" },
                    { 15, "AW", true, "Aruba" },
                    { 16, "AZ", true, "Azerbaijan" },
                    { 17, "BA", true, "Bosnia and Herzegovina" },
                    { 18, "BB", true, "Barbados" },
                    { 19, "BD", true, "Bangladesh" },
                    { 20, "BE", true, "Belgium" },
                    { 21, "BF", true, "Burkina Faso" },
                    { 22, "BG", true, "Bulgaria" },
                    { 23, "BH", true, "Bahrain" },
                    { 24, "BI", true, "Burundi" },
                    { 25, "BJ", true, "Benin" },
                    { 26, "BM", true, "Bermuda" },
                    { 27, "BN", true, "Brunei" },
                    { 28, "BO", true, "Bolivia" },
                    { 29, "BR", true, "Brazil" },
                    { 30, "BS", true, "Bahamas" },
                    { 31, "BT", true, "Bhutan" },
                    { 32, "BV", true, "Bouvet Island" },
                    { 33, "BW", true, "Botswana" },
                    { 34, "BY", true, "Belarus" },
                    { 35, "BZ", true, "Belize" },
                    { 36, "CA", true, "Canada" },
                    { 37, "CC", true, "Cocos (Keeling) Islands" },
                    { 38, "CD", true, "Congo (DRC)" },
                    { 39, "CF", true, "Central African Republic" },
                    { 40, "CG", true, "Congo (Republic)" },
                    { 41, "CH", true, "Switzerland" },
                    { 42, "CI", true, "Côte d'Ivoire" },
                    { 43, "CK", true, "Cook Islands" },
                    { 44, "CL", true, "Chile" },
                    { 45, "CM", true, "Cameroon" },
                    { 46, "CN", true, "China" },
                    { 47, "CO", true, "Colombia" },
                    { 48, "CR", true, "Costa Rica" },
                    { 49, "CU", true, "Cuba" },
                    { 50, "CV", true, "Cape Verde" },
                    { 51, "CX", true, "Christmas Island" },
                    { 52, "CY", true, "Cyprus" },
                    { 53, "CZ", true, "Czech Republic" },
                    { 54, "DE", true, "Germany" },
                    { 55, "DJ", true, "Djibouti" },
                    { 56, "DK", true, "Denmark" },
                    { 57, "DM", true, "Dominica" },
                    { 58, "DO", true, "Dominican Republic" },
                    { 59, "DZ", true, "Algeria" },
                    { 60, "EC", true, "Ecuador" },
                    { 61, "EE", true, "Estonia" },
                    { 62, "EG", true, "Egypt" },
                    { 63, "EH", true, "Western Sahara" },
                    { 64, "ER", true, "Eritrea" },
                    { 65, "ES", true, "Spain" },
                    { 66, "ET", true, "Ethiopia" },
                    { 67, "FI", true, "Finland" },
                    { 68, "FJ", true, "Fiji" },
                    { 69, "FK", true, "Falkland Islands" },
                    { 70, "FM", true, "Micronesia" },
                    { 71, "FO", true, "Faroe Islands" },
                    { 72, "FR", true, "France" },
                    { 73, "GA", true, "Gabon" },
                    { 74, "GB", true, "United Kingdom" },
                    { 75, "GD", true, "Grenada" },
                    { 76, "GE", true, "Georgia" },
                    { 77, "GF", true, "French Guiana" },
                    { 78, "GG", true, "Guernsey" },
                    { 79, "GH", true, "Ghana" },
                    { 80, "GI", true, "Gibraltar" },
                    { 81, "GL", true, "Greenland" },
                    { 82, "GM", true, "Gambia" },
                    { 83, "GN", true, "Guinea" },
                    { 84, "GP", true, "Guadeloupe" },
                    { 85, "GQ", true, "Equatorial Guinea" },
                    { 86, "GR", true, "Greece" },
                    { 87, "GT", true, "Guatemala" },
                    { 88, "GU", true, "Guam" },
                    { 89, "GW", true, "Guinea-Bissau" },
                    { 90, "GY", true, "Guyana" },
                    { 91, "HK", true, "Hong Kong" },
                    { 92, "HN", true, "Honduras" },
                    { 93, "HR", true, "Croatia" },
                    { 94, "HT", true, "Haiti" },
                    { 95, "HU", true, "Hungary" },
                    { 96, "ID", true, "Indonesia" },
                    { 97, "IE", true, "Ireland" },
                    { 98, "IL", true, "Israel" },
                    { 99, "IN", true, "India" },
                    { 100, "IQ", true, "Iraq" },
                    { 101, "IR", true, "Iran" },
                    { 102, "IS", true, "Iceland" },
                    { 103, "IT", true, "Italy" },
                    { 104, "JM", true, "Jamaica" },
                    { 105, "JO", true, "Jordan" },
                    { 106, "JP", true, "Japan" },
                    { 107, "KE", true, "Kenya" },
                    { 108, "KH", true, "Cambodia" },
                    { 109, "KR", true, "South Korea" },
                    { 110, "KW", true, "Kuwait" },
                    { 111, "KZ", true, "Kazakhstan" },
                    { 112, "LA", true, "Laos" },
                    { 113, "LB", true, "Lebanon" },
                    { 114, "LK", true, "Sri Lanka" },
                    { 115, "LR", true, "Liberia" },
                    { 116, "LS", true, "Lesotho" },
                    { 117, "LT", true, "Lithuania" },
                    { 118, "LU", true, "Luxembourg" },
                    { 119, "LV", true, "Latvia" },
                    { 120, "LY", true, "Libya" },
                    { 121, "MA", true, "Morocco" },
                    { 122, "MC", true, "Monaco" },
                    { 123, "MD", true, "Moldova" },
                    { 124, "ME", true, "Montenegro" },
                    { 125, "MG", true, "Madagascar" },
                    { 126, "MV", true, "Maldives" },
                    { 127, "MX", true, "Mexico" },
                    { 128, "MY", true, "Malaysia" },
                    { 129, "MZ", true, "Mozambique" },
                    { 130, "NA", true, "Namibia" },
                    { 131, "NG", true, "Nigeria" },
                    { 132, "NL", true, "Netherlands" },
                    { 133, "NO", true, "Norway" },
                    { 134, "NP", true, "Nepal" },
                    { 135, "NZ", true, "New Zealand" },
                    { 136, "OM", true, "Oman" },
                    { 137, "PA", true, "Panama" },
                    { 138, "PE", true, "Peru" },
                    { 139, "PH", true, "Philippines" },
                    { 140, "PK", true, "Pakistan" },
                    { 141, "PL", true, "Poland" },
                    { 142, "PT", true, "Portugal" },
                    { 143, "QA", true, "Qatar" },
                    { 144, "RO", true, "Romania" },
                    { 145, "RS", true, "Serbia" },
                    { 146, "RU", true, "Russia" },
                    { 147, "RW", true, "Rwanda" },
                    { 148, "SA", true, "Saudi Arabia" },
                    { 149, "SE", true, "Sweden" },
                    { 150, "SG", true, "Singapore" },
                    { 151, "SI", true, "Slovenia" },
                    { 152, "SK", true, "Slovakia" },
                    { 153, "SN", true, "Senegal" },
                    { 154, "SO", true, "Somalia" },
                    { 155, "SR", true, "Suriname" },
                    { 156, "SV", true, "El Salvador" },
                    { 157, "SY", true, "Syria" },
                    { 158, "TH", true, "Thailand" },
                    { 159, "TJ", true, "Tajikistan" },
                    { 160, "TL", true, "Timor-Leste" },
                    { 161, "TM", true, "Turkmenistan" },
                    { 162, "TN", true, "Tunisia" },
                    { 163, "TR", true, "Turkey" },
                    { 164, "TW", true, "Taiwan" },
                    { 165, "TZ", true, "Tanzania" },
                    { 166, "UA", true, "Ukraine" },
                    { 167, "UG", true, "Uganda" },
                    { 168, "US", true, "United States" },
                    { 169, "UY", true, "Uruguay" },
                    { 170, "UZ", true, "Uzbekistan" },
                    { 171, "VA", true, "Vatican City" },
                    { 172, "VE", true, "Venezuela" },
                    { 173, "VN", true, "Vietnam" },
                    { 174, "YE", true, "Yemen" },
                    { 175, "ZA", true, "South Africa" },
                    { 176, "ZM", true, "Zambia" },
                    { 177, "ZW", true, "Zimbabwe" },
                    { 178, "", true, "Default" }
                });

            migrationBuilder.InsertData(
                schema: "location",
                table: "Locations",
                columns: new[] { "id", "country_id", "description", "is_active", "name" },
                values: new object[] { 1, 178, "Main location descriptions", true, "Main Location" });

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
