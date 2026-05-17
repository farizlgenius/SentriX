using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adapter.Aero.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDriverTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "aero",
                table: "DriverConfigurations",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "aero",
                table: "SioPanelConfigurations",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "device_id",
                schema: "aero",
                table: "SioPanelConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "mac",
                schema: "aero",
                table: "OutputPointSpecifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "device_id",
                schema: "aero",
                table: "DriverConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "mac",
                schema: "aero",
                table: "ControlPointConfigurations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "device_id",
                schema: "aero",
                table: "SioPanelConfigurations");

            migrationBuilder.DropColumn(
                name: "mac",
                schema: "aero",
                table: "OutputPointSpecifications");

            migrationBuilder.DropColumn(
                name: "device_id",
                schema: "aero",
                table: "DriverConfigurations");

            migrationBuilder.DropColumn(
                name: "mac",
                schema: "aero",
                table: "ControlPointConfigurations");

            migrationBuilder.InsertData(
                schema: "aero",
                table: "DriverConfigurations",
                columns: new[] { "id", "baudrate", "is_active", "mac", "msp1_number", "n_dialect", "n_protocol", "port_number", "reply_time", "scp_id" },
                values: new object[] { 1, (short)-1, true, "", (short)0, (short)0, (short)0, (short)3, (short)0, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "SioPanelConfigurations",
                columns: new[] { "id", "address", "emax", "enable", "flags", "is_active", "mac", "model", "module_id", "n_inputs", "n_outputs", "n_readers", "n_sio_next_in", "n_sio_next_out", "n_sio_next_rdr", "port", "scp_id", "sio_number" },
                values: new object[] { 1, (short)0, (short)3, (short)1, (short)0, true, "", (short)196, 0, (short)7, (short)4, (short)4, (short)-1, (short)-1, (short)-1, (short)0, (short)0, (short)0 });
        }
    }
}
