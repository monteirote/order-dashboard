using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnsInDecorPrints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_Backings_BackingId",
                table: "DecorPrints");

            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_Frames_FrameId",
                table: "DecorPrints");

            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_GlassTypes_GlassTypeId",
                table: "DecorPrints");

            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_Papers_PaperId",
                table: "DecorPrints");

            migrationBuilder.AlterColumn<int>(
                name: "PaperId",
                table: "DecorPrints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GlassTypeId",
                table: "DecorPrints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FrameId",
                table: "DecorPrints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BackingId",
                table: "DecorPrints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_Backings_BackingId",
                table: "DecorPrints",
                column: "BackingId",
                principalTable: "Backings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_Frames_FrameId",
                table: "DecorPrints",
                column: "FrameId",
                principalTable: "Frames",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_GlassTypes_GlassTypeId",
                table: "DecorPrints",
                column: "GlassTypeId",
                principalTable: "GlassTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_Papers_PaperId",
                table: "DecorPrints",
                column: "PaperId",
                principalTable: "Papers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_Backings_BackingId",
                table: "DecorPrints");

            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_Frames_FrameId",
                table: "DecorPrints");

            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_GlassTypes_GlassTypeId",
                table: "DecorPrints");

            migrationBuilder.DropForeignKey(
                name: "FK_DecorPrints_Papers_PaperId",
                table: "DecorPrints");

            migrationBuilder.AlterColumn<int>(
                name: "PaperId",
                table: "DecorPrints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GlassTypeId",
                table: "DecorPrints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FrameId",
                table: "DecorPrints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BackingId",
                table: "DecorPrints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_Backings_BackingId",
                table: "DecorPrints",
                column: "BackingId",
                principalTable: "Backings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_Frames_FrameId",
                table: "DecorPrints",
                column: "FrameId",
                principalTable: "Frames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_GlassTypes_GlassTypeId",
                table: "DecorPrints",
                column: "GlassTypeId",
                principalTable: "GlassTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DecorPrints_Papers_PaperId",
                table: "DecorPrints",
                column: "PaperId",
                principalTable: "Papers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
