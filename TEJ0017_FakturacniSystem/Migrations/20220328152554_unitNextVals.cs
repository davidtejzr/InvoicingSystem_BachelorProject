using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TEJ0017_FakturacniSystem.Migrations
{
    public partial class unitNextVals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Vat",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "defaultUnit",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "defaultUnit",
                table: "Items");
        }
    }
}
