using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderDashboard.Migrations
{
    /// <inheritdoc />
    public partial class NewChangesOnDecorPrintsAndAddingServiceOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_Backings_BackingId",
                table: "DecorPrints");

            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_Papers_PaperId",
                table: "DecorPrints");

            migrationBuilder.DropTable(
                name: "Backings");

            migrationBuilder.DropTable(
                name: "Papers");

            migrationBuilder.DropIndex(
                name: "IX_DecorPrints_BackingId",
                table: "DecorPrints");

            migrationBuilder.DropIndex(
                name: "IX_DecorPrints_PaperId",
                table: "DecorPrints");

            migrationBuilder.DropColumn(
                name: "BackingId",
                table: "DecorPrints");

            migrationBuilder.DropColumn(
                name: "PaperId",
                table: "DecorPrints");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DecorPrints",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ServiceOrderId",
                table: "DecorPrints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ServiceOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CustomerName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOrders", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DecorPrints_ServiceOrderId",
                table: "DecorPrints",
                column: "ServiceOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_ServiceOrders_ServiceOrderId",
                table: "DecorPrints",
                column: "ServiceOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_ServiceOrders_ServiceOrderId",
                table: "DecorPrints");

            migrationBuilder.DropTable(
                name: "ServiceOrders");

            migrationBuilder.DropIndex(
                name: "IX_DecorPrints_ServiceOrderId",
                table: "DecorPrints");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DecorPrints");

            migrationBuilder.DropColumn(
                name: "ServiceOrderId",
                table: "DecorPrints");

            migrationBuilder.AddColumn<int>(
                name: "BackingId",
                table: "DecorPrints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaperId",
                table: "DecorPrints",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Backings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backings", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Papers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Papers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DecorPrints_BackingId",
                table: "DecorPrints",
                column: "BackingId");

            migrationBuilder.CreateIndex(
                name: "IX_DecorPrints_PaperId",
                table: "DecorPrints",
                column: "PaperId");

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_Backings_BackingId",
                table: "DecorPrints",
                column: "BackingId",
                principalTable: "Backings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_Papers_PaperId",
                table: "DecorPrints",
                column: "PaperId",
                principalTable: "Papers",
                principalColumn: "Id");
        }
    }
}
