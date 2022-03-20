using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TEJ0017_FakturacniSystem.Migrations
{
    public partial class documnetsids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_BankDetails_BankDetailPaymentMethodId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Customers_CustomerSubjectId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_PaymentMethods_PaymentMethodId",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodId",
                table: "Documents",
                newName: "PaymentmethodId");

            migrationBuilder.RenameColumn(
                name: "CustomerSubjectId",
                table: "Documents",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "BankDetailPaymentMethodId",
                table: "Documents",
                newName: "BankDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_PaymentMethodId",
                table: "Documents",
                newName: "IX_Documents_PaymentmethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CustomerSubjectId",
                table: "Documents",
                newName: "IX_Documents_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_BankDetailPaymentMethodId",
                table: "Documents",
                newName: "IX_Documents_BankDetailId");

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

            migrationBuilder.RenameColumn(
                name: "PaymentmethodId",
                table: "Documents",
                newName: "PaymentMethodId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Documents",
                newName: "CustomerSubjectId");

            migrationBuilder.RenameColumn(
                name: "BankDetailId",
                table: "Documents",
                newName: "BankDetailPaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_PaymentmethodId",
                table: "Documents",
                newName: "IX_Documents_PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CustomerId",
                table: "Documents",
                newName: "IX_Documents_CustomerSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_BankDetailId",
                table: "Documents",
                newName: "IX_Documents_BankDetailPaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_BankDetails_BankDetailPaymentMethodId",
                table: "Documents",
                column: "BankDetailPaymentMethodId",
                principalTable: "BankDetails",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Customers_CustomerSubjectId",
                table: "Documents",
                column: "CustomerSubjectId",
                principalTable: "Customers",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_PaymentMethods_PaymentMethodId",
                table: "Documents",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
