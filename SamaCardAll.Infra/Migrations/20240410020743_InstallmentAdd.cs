using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamaCardAll.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InstallmentAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Cards",
            //    columns: table => new
            //    {
            //        IdCard = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Bank = table.Column<string>(type: "TEXT", nullable: false),
            //        Number = table.Column<string>(type: "TEXT", nullable: false),
            //        Expiration = table.Column<string>(type: "TEXT", nullable: false),
            //        Brand = table.Column<string>(type: "TEXT", nullable: false),
            //        Active = table.Column<short>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Cards", x => x.IdCard);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Customers",
            //    columns: table => new
            //    {
            //        IdCustomer = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        CustomerName = table.Column<string>(type: "TEXT", nullable: true),
            //        Active = table.Column<short>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Customers", x => x.IdCustomer);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        IdUser = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Name = table.Column<string>(type: "TEXT", nullable: true),
            //        Password = table.Column<string>(type: "TEXT", nullable: true),
            //        Active = table.Column<short>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.IdUser);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Spends",
            //    columns: table => new
            //    {
            //        IdSpend = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Expenses = table.Column<string>(type: "TEXT", nullable: true),
            //        Amount = table.Column<decimal>(type: "TEXT", nullable: false),
            //        Date = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        InstallmentPlan = table.Column<int>(type: "INTEGER", nullable: false),
            //        InstallmentValue = table.Column<decimal>(type: "TEXT", nullable: false),
            //        Deleted = table.Column<short>(type: "INTEGER", nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        CustomerIdCustomer = table.Column<int>(type: "INTEGER", nullable: false),
            //        CardIdCard = table.Column<int>(type: "INTEGER", nullable: false),
            //        UserIdUser = table.Column<int>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Spends", x => x.IdSpend);
            //        table.ForeignKey(
            //            name: "FK_Spends_Cards_CardIdCard",
            //            column: x => x.CardIdCard,
            //            principalTable: "Cards",
            //            principalColumn: "IdCard",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Spends_Customers_CustomerIdCustomer",
            //            column: x => x.CustomerIdCustomer,
            //            principalTable: "Customers",
            //            principalColumn: "IdCustomer",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Spends_Users_UserIdUser",
            //            column: x => x.UserIdUser,
            //            principalTable: "Users",
            //            principalColumn: "IdUser",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "Installments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonthYear = table.Column<string>(type: "TEXT", nullable: false),
                    InstallmentValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Active = table.Column<short>(type: "INTEGER", nullable: false),
                    IdSpend = table.Column<int>(type: "INTEGER", nullable: false),
                    SpendIdSpend = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Installments_Spends_SpendIdSpend",
                        column: x => x.SpendIdSpend,
                        principalTable: "Spends",
                        principalColumn: "IdSpend");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Installments_SpendIdSpend",
                table: "Installments",
                column: "SpendIdSpend");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Spends_CardIdCard",
            //    table: "Spends",
            //    column: "CardIdCard");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Spends_CustomerIdCustomer",
            //    table: "Spends",
            //    column: "CustomerIdCustomer");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Spends_UserIdUser",
            //    table: "Spends",
            //    column: "UserIdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Installments");

            migrationBuilder.DropTable(
                name: "Spends");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
