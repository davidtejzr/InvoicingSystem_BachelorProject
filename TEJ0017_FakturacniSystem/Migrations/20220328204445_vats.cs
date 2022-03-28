using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TEJ0017_FakturacniSystem.Migrations
{
    public partial class vats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "DocumentItems",
                newName: "Vat");

            migrationBuilder.AddColumn<float>(
                name: "PriceWoVat",
                table: "DocumentItems",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceWoVat",
                table: "DocumentItems");

            migrationBuilder.RenameColumn(
                name: "Vat",
                table: "DocumentItems",
                newName: "TaxRate");
        }
    }
}
