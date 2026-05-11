using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocation_operators_operator_id",
                schema: "operator",
                table: "OperatorLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperatorLocation",
                schema: "operator",
                table: "OperatorLocation");

            migrationBuilder.DropColumn(
                name: "company_id",
                schema: "operator",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "department_id",
                schema: "operator",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "position_id",
                schema: "operator",
                table: "operators");

            migrationBuilder.RenameTable(
                name: "OperatorLocation",
                schema: "operator",
                newName: "operator_locations",
                newSchema: "operator");

            migrationBuilder.RenameIndex(
                name: "IX_OperatorLocation_operator_id",
                schema: "operator",
                table: "operator_locations",
                newName: "IX_operator_locations_operator_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_operator_locations",
                schema: "operator",
                table: "operator_locations",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_operator_locations_operators_operator_id",
                schema: "operator",
                table: "operator_locations",
                column: "operator_id",
                principalSchema: "operator",
                principalTable: "operators",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_operator_locations_operators_operator_id",
                schema: "operator",
                table: "operator_locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_operator_locations",
                schema: "operator",
                table: "operator_locations");

            migrationBuilder.RenameTable(
                name: "operator_locations",
                schema: "operator",
                newName: "OperatorLocation",
                newSchema: "operator");

            migrationBuilder.RenameIndex(
                name: "IX_operator_locations_operator_id",
                schema: "operator",
                table: "OperatorLocation",
                newName: "IX_OperatorLocation_operator_id");

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                schema: "operator",
                table: "operators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "department_id",
                schema: "operator",
                table: "operators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "position_id",
                schema: "operator",
                table: "operators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperatorLocation",
                schema: "operator",
                table: "OperatorLocation",
                column: "id");

            migrationBuilder.UpdateData(
                schema: "operator",
                table: "operators",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "company_id", "department_id", "position_id" },
                values: new object[] { 0, 0, 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocation_operators_operator_id",
                schema: "operator",
                table: "OperatorLocation",
                column: "operator_id",
                principalSchema: "operator",
                principalTable: "operators",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
