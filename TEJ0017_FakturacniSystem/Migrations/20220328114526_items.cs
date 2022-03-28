using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TEJ0017_FakturacniSystem.Migrations
{
    public partial class items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentItems_TaxRate_TaxRateId",
                table: "DocumentItems");

            migrationBuilder.DropTable(
                name: "TaxRate");

            migrationBuilder.DropIndex(
                name: "IX_DocumentItems_TaxRateId",
                table: "DocumentItems");

            migrationBuilder.RenameColumn(
                name: "TaxRateId",
                table: "DocumentItems",
                newName: "TaxRate");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceWoVat = table.Column<float>(type: "real", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "DocumentItems",
                newName: "TaxRateId");

            migrationBuilder.CreateTable(
                name: "TaxRate",
                columns: table => new
                {
                    TaxRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRate", x => x.TaxRateId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentItems_TaxRateId",
                table: "DocumentItems",
                column: "TaxRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentItems_TaxRate_TaxRateId",
                table: "DocumentItems",
                column: "TaxRateId",
                principalTable: "TaxRate",
                principalColumn: "TaxRateId");
        }
    }
}
