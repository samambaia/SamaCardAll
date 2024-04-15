using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamaCardAll.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installments_Spends_SpendIdSpend",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "IdSpend",
                table: "Installments");

            migrationBuilder.AlterColumn<int>(
                name: "SpendIdSpend",
                table: "Installments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_Spends_SpendIdSpend",
                table: "Installments",
                column: "SpendIdSpend",
                principalTable: "Spends",
                principalColumn: "IdSpend",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installments_Spends_SpendIdSpend",
                table: "Installments");

            migrationBuilder.AlterColumn<int>(
                name: "SpendIdSpend",
                table: "Installments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "IdSpend",
                table: "Installments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_Spends_SpendIdSpend",
                table: "Installments",
                column: "SpendIdSpend",
                principalTable: "Spends",
                principalColumn: "IdSpend");
        }
    }
}
