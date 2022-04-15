using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TEJ0017_FakturacniSystem.Migrations
{
    public partial class remTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceTemplates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceTemplates",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTemplates", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_InvoiceTemplates_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId");
                });
        }
    }
}
