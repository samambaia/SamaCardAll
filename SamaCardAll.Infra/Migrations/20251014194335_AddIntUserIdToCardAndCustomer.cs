using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamaCardAll.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddIntUserIdToCardAndCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "UserIdUser",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "UserIdUser",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserIdUser",
                table: "RefreshTokens",
                column: "UserIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserIdUser",
                table: "Customers",
                column: "UserIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserIdUser",
                table: "Cards",
                column: "UserIdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Users_UserIdUser",
                table: "Cards",
                column: "UserIdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_UserIdUser",
                table: "Customers",
                column: "UserIdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserIdUser",
                table: "RefreshTokens",
                column: "UserIdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Users_UserIdUser",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_UserIdUser",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserIdUser",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Customers_UserIdUser",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Cards_UserIdUser",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "UserIdUser",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserIdUser",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UserIdUser",
                table: "Cards");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Spends_UserId",
                table: "Spends",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Spends_Users_UserId",
                table: "Spends",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
