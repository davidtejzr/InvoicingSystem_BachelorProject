using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TEJ0017_FakturacniSystem.Migrations
{
    public partial class rltship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_BankDetails_BankDetailId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Customers_CustomerId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_PaymentMethods_PaymentmethodId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentmethodId",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BankDetailId",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_BankDetails_BankDetailId",
                table: "Documents",
                column: "BankDetailId",
                principalTable: "BankDetails",
                principalColumn: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Customers_CustomerId",
                table: "Documents",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_PaymentMethods_PaymentmethodId",
                table: "Documents",
                column: "PaymentmethodId",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_BankDetails_BankDetailId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Customers_CustomerId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_PaymentMethods_PaymentmethodId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentmethodId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BankDetailId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_BankDetails_BankDetailId",
                table: "Documents",
                column: "BankDetailId",
                principalTable: "BankDetails",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Customers_CustomerId",
                table: "Documents",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_PaymentMethods_PaymentmethodId",
                table: "Documents",
                column: "PaymentmethodId",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
