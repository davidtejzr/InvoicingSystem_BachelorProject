using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TEJ0017_FakturacniSystem.Migrations
{
    public partial class invoiceItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentItems_Documents_DocumentId",
                table: "DocumentItems");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "DocumentItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentItems_Documents_DocumentId",
                table: "DocumentItems",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentItems_Documents_DocumentId",
                table: "DocumentItems");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "DocumentItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentItems_Documents_DocumentId",
                table: "DocumentItems",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId");
        }
    }
}
