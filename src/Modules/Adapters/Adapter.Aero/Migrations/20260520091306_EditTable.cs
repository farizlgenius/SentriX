using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Adapter.Aero.Migrations
{
    /// <inheritdoc />
    public partial class EditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlPointConfigurations_Aeros_aero_id",
                schema: "aero",
                table: "ControlPointConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverConfigurations_Aeros_aero_id",
                schema: "aero",
                table: "DriverConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_InputPointSpecifications_Aeros_aero_id",
                schema: "aero",
                table: "InputPointSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputPointSpecifications_Aeros_aero_id",
                schema: "aero",
                table: "OutputPointSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_SioPanelConfigurations_Aeros_aero_id",
                schema: "aero",
                table: "SioPanelConfigurations");

            migrationBuilder.DropTable(
                name: "CreateChannels",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "SystemLevelSpecifications",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "WriterAudits",
                schema: "aero");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SioPanelConfigurations",
                schema: "aero",
                table: "SioPanelConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutputPointSpecifications",
                schema: "aero",
                table: "OutputPointSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InputPointSpecifications",
                schema: "aero",
                table: "InputPointSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverConfigurations",
                schema: "aero",
                table: "DriverConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControlPointConfigurations",
                schema: "aero",
                table: "ControlPointConfigurations");

            migrationBuilder.RenameTable(
                name: "SioPanelConfigurations",
                schema: "aero",
                newName: "SioPanelConfiguration",
                newSchema: "aero");

            migrationBuilder.RenameTable(
                name: "OutputPointSpecifications",
                schema: "aero",
                newName: "OutputPointSpecification",
                newSchema: "aero");

            migrationBuilder.RenameTable(
                name: "InputPointSpecifications",
                schema: "aero",
                newName: "InputPointSpecification",
                newSchema: "aero");

            migrationBuilder.RenameTable(
                name: "DriverConfigurations",
                schema: "aero",
                newName: "DriverConfiguration",
                newSchema: "aero");

            migrationBuilder.RenameTable(
                name: "ControlPointConfigurations",
                schema: "aero",
                newName: "ControlPointConfiguration",
                newSchema: "aero");

            migrationBuilder.RenameIndex(
                name: "IX_SioPanelConfigurations_aero_id",
                schema: "aero",
                table: "SioPanelConfiguration",
                newName: "IX_SioPanelConfiguration_aero_id");

            migrationBuilder.RenameIndex(
                name: "IX_OutputPointSpecifications_aero_id",
                schema: "aero",
                table: "OutputPointSpecification",
                newName: "IX_OutputPointSpecification_aero_id");

            migrationBuilder.RenameIndex(
                name: "IX_InputPointSpecifications_aero_id",
                schema: "aero",
                table: "InputPointSpecification",
                newName: "IX_InputPointSpecification_aero_id");

            migrationBuilder.RenameIndex(
                name: "IX_DriverConfigurations_aero_id",
                schema: "aero",
                table: "DriverConfiguration",
                newName: "IX_DriverConfiguration_aero_id");

            migrationBuilder.RenameIndex(
                name: "IX_ControlPointConfigurations_aero_id",
                schema: "aero",
                table: "ControlPointConfiguration",
                newName: "IX_ControlPointConfiguration_aero_id");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "ScpDeviceSpecifications",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "aero",
                table: "ScpDeviceSpecifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "Aeros",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "Aeros",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "Aeros",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "AccessDatabaseSpecifications",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "aero",
                table: "AccessDatabaseSpecifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "SioPanelConfiguration",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "SioPanelConfiguration",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "SioPanelConfiguration",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "aero",
                table: "SioPanelConfiguration",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "OutputPointSpecification",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "OutputPointSpecification",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "OutputPointSpecification",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "aero",
                table: "OutputPointSpecification",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "InputPointSpecification",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "InputPointSpecification",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "InputPointSpecification",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "aero",
                table: "InputPointSpecification",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "DriverConfiguration",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "DriverConfiguration",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "DriverConfiguration",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "aero",
                table: "DriverConfiguration",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "ControlPointConfiguration",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "ControlPointConfiguration",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<short>(
                name: "component_id",
                schema: "aero",
                table: "ControlPointConfiguration",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                schema: "aero",
                table: "ControlPointConfiguration",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "output_id",
                schema: "aero",
                table: "ControlPointConfiguration",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SioPanelConfiguration",
                schema: "aero",
                table: "SioPanelConfiguration",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutputPointSpecification",
                schema: "aero",
                table: "OutputPointSpecification",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InputPointSpecification",
                schema: "aero",
                table: "InputPointSpecification",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverConfiguration",
                schema: "aero",
                table: "DriverConfiguration",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControlPointConfiguration",
                schema: "aero",
                table: "ControlPointConfiguration",
                column: "id");

            migrationBuilder.UpdateData(
                schema: "aero",
                table: "AccessDatabaseSpecifications",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.UpdateData(
                schema: "aero",
                table: "ScpDeviceSpecifications",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "component_id", "location_id" },
                values: new object[] { (short)0, 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_ControlPointConfiguration_Aeros_aero_id",
                schema: "aero",
                table: "ControlPointConfiguration",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverConfiguration_Aeros_aero_id",
                schema: "aero",
                table: "DriverConfiguration",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InputPointSpecification_Aeros_aero_id",
                schema: "aero",
                table: "InputPointSpecification",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputPointSpecification_Aeros_aero_id",
                schema: "aero",
                table: "OutputPointSpecification",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SioPanelConfiguration_Aeros_aero_id",
                schema: "aero",
                table: "SioPanelConfiguration",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlPointConfiguration_Aeros_aero_id",
                schema: "aero",
                table: "ControlPointConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverConfiguration_Aeros_aero_id",
                schema: "aero",
                table: "DriverConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_InputPointSpecification_Aeros_aero_id",
                schema: "aero",
                table: "InputPointSpecification");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputPointSpecification_Aeros_aero_id",
                schema: "aero",
                table: "OutputPointSpecification");

            migrationBuilder.DropForeignKey(
                name: "FK_SioPanelConfiguration_Aeros_aero_id",
                schema: "aero",
                table: "SioPanelConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SioPanelConfiguration",
                schema: "aero",
                table: "SioPanelConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutputPointSpecification",
                schema: "aero",
                table: "OutputPointSpecification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InputPointSpecification",
                schema: "aero",
                table: "InputPointSpecification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverConfiguration",
                schema: "aero",
                table: "DriverConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControlPointConfiguration",
                schema: "aero",
                table: "ControlPointConfiguration");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "ScpDeviceSpecifications");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "aero",
                table: "ScpDeviceSpecifications");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "aero",
                table: "ElevatorAccessLevelSpecifications");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "Aeros");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "AccessDatabaseSpecifications");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "aero",
                table: "AccessDatabaseSpecifications");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "SioPanelConfiguration");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "aero",
                table: "SioPanelConfiguration");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "OutputPointSpecification");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "aero",
                table: "OutputPointSpecification");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "InputPointSpecification");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "aero",
                table: "InputPointSpecification");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "DriverConfiguration");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "aero",
                table: "DriverConfiguration");

            migrationBuilder.DropColumn(
                name: "component_id",
                schema: "aero",
                table: "ControlPointConfiguration");

            migrationBuilder.DropColumn(
                name: "location_id",
                schema: "aero",
                table: "ControlPointConfiguration");

            migrationBuilder.DropColumn(
                name: "output_id",
                schema: "aero",
                table: "ControlPointConfiguration");

            migrationBuilder.RenameTable(
                name: "SioPanelConfiguration",
                schema: "aero",
                newName: "SioPanelConfigurations",
                newSchema: "aero");

            migrationBuilder.RenameTable(
                name: "OutputPointSpecification",
                schema: "aero",
                newName: "OutputPointSpecifications",
                newSchema: "aero");

            migrationBuilder.RenameTable(
                name: "InputPointSpecification",
                schema: "aero",
                newName: "InputPointSpecifications",
                newSchema: "aero");

            migrationBuilder.RenameTable(
                name: "DriverConfiguration",
                schema: "aero",
                newName: "DriverConfigurations",
                newSchema: "aero");

            migrationBuilder.RenameTable(
                name: "ControlPointConfiguration",
                schema: "aero",
                newName: "ControlPointConfigurations",
                newSchema: "aero");

            migrationBuilder.RenameIndex(
                name: "IX_SioPanelConfiguration_aero_id",
                schema: "aero",
                table: "SioPanelConfigurations",
                newName: "IX_SioPanelConfigurations_aero_id");

            migrationBuilder.RenameIndex(
                name: "IX_OutputPointSpecification_aero_id",
                schema: "aero",
                table: "OutputPointSpecifications",
                newName: "IX_OutputPointSpecifications_aero_id");

            migrationBuilder.RenameIndex(
                name: "IX_InputPointSpecification_aero_id",
                schema: "aero",
                table: "InputPointSpecifications",
                newName: "IX_InputPointSpecifications_aero_id");

            migrationBuilder.RenameIndex(
                name: "IX_DriverConfiguration_aero_id",
                schema: "aero",
                table: "DriverConfigurations",
                newName: "IX_DriverConfigurations_aero_id");

            migrationBuilder.RenameIndex(
                name: "IX_ControlPointConfiguration_aero_id",
                schema: "aero",
                table: "ControlPointConfigurations",
                newName: "IX_ControlPointConfigurations_aero_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "Aeros",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "Aeros",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "SioPanelConfigurations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "SioPanelConfigurations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "OutputPointSpecifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "OutputPointSpecifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "InputPointSpecifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "InputPointSpecifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "DriverConfigurations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "DriverConfigurations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "aero",
                table: "ControlPointConfigurations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "aero",
                table: "ControlPointConfigurations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SioPanelConfigurations",
                schema: "aero",
                table: "SioPanelConfigurations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutputPointSpecifications",
                schema: "aero",
                table: "OutputPointSpecifications",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InputPointSpecifications",
                schema: "aero",
                table: "InputPointSpecifications",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverConfigurations",
                schema: "aero",
                table: "DriverConfigurations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControlPointConfigurations",
                schema: "aero",
                table: "ControlPointConfigurations",
                column: "id");

            migrationBuilder.CreateTable(
                name: "CreateChannels",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    baudrate = table.Column<short>(type: "smallint", nullable: false),
                    c_model_id = table.Column<short>(type: "smallint", nullable: false),
                    c_port = table.Column<short>(type: "smallint", nullable: false),
                    c_rts_mode = table.Column<short>(type: "smallint", nullable: false),
                    c_type = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    n_channel_id = table.Column<short>(type: "smallint", nullable: false),
                    timer_1 = table.Column<short>(type: "smallint", nullable: false),
                    timer_2 = table.Column<short>(type: "smallint", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateChannels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SystemLevelSpecifications",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    b_direct_mode = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    debug_rq = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    n_debug_arg = table.Column<short>(type: "smallint", nullable: false),
                    n_holidays = table.Column<short>(type: "smallint", nullable: false),
                    n_ports = table.Column<short>(type: "smallint", nullable: false),
                    n_scps = table.Column<short>(type: "smallint", nullable: false),
                    n_timezones = table.Column<short>(type: "smallint", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLevelSpecifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WriterAudits",
                schema: "aero",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    body = table.Column<string>(type: "text", nullable: false),
                    command = table.Column<string>(type: "text", nullable: false),
                    command_code = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_nak = table.Column<bool>(type: "boolean", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    received_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    scp_id = table.Column<int>(type: "integer", nullable: false),
                    send_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    tag = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriterAudits", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "CreateChannels",
                columns: new[] { "id", "baudrate", "c_model_id", "c_port", "c_rts_mode", "c_type", "is_active", "n_channel_id", "timer_1", "timer_2" },
                values: new object[] { 1, (short)0, (short)0, (short)0, (short)0, (short)7, true, (short)1, (short)3000, (short)0 });

            migrationBuilder.InsertData(
                schema: "aero",
                table: "SystemLevelSpecifications",
                columns: new[] { "id", "b_direct_mode", "debug_rq", "is_active", "n_debug_arg", "n_holidays", "n_ports", "n_scps", "n_timezones" },
                values: new object[] { 1, (short)1, (short)0, true, (short)0, (short)0, (short)1024, (short)1024, (short)0 });

            migrationBuilder.AddForeignKey(
                name: "FK_ControlPointConfigurations_Aeros_aero_id",
                schema: "aero",
                table: "ControlPointConfigurations",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverConfigurations_Aeros_aero_id",
                schema: "aero",
                table: "DriverConfigurations",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InputPointSpecifications_Aeros_aero_id",
                schema: "aero",
                table: "InputPointSpecifications",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputPointSpecifications_Aeros_aero_id",
                schema: "aero",
                table: "OutputPointSpecifications",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SioPanelConfigurations_Aeros_aero_id",
                schema: "aero",
                table: "SioPanelConfigurations",
                column: "aero_id",
                principalSchema: "aero",
                principalTable: "Aeros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
