﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TEJ0017_FakturacniSystem.Models;

#nullable disable

namespace TEJ0017_FakturacniSystem.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20220319220119_bankDetail")]
    partial class bankDetail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentId"), 1L, 1);

                    b.Property<int>("BankDetailPaymentMethodId")
                        .HasColumnType("int");

                    b.Property<string>("ConstantSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerSubjectId")
                        .HasColumnType("int");

                    b.Property<float?>("Discount")
                        .HasColumnType("real");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TaxDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("TotalAmount")
                        .HasColumnType("real");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("VariableSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("footerDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("headerDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId");

                    b.HasIndex("BankDetailPaymentMethodId");

                    b.HasIndex("CustomerSubjectId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("UserId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentItem", b =>
                {
                    b.Property<int>("DocumentItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentItemId"), 1L, 1);

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TaxRateId")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("UnitPrice")
                        .HasColumnType("real");

                    b.HasKey("DocumentItemId");

                    b.HasIndex("DocumentId");

                    b.HasIndex("TaxRateId");

                    b.ToTable("DocumentItems");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.TaxRate", b =>
                {
                    b.Property<int>("TaxRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaxRateId"), 1L, 1);

                    b.Property<string>("GoodsTypes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Rate")
                        .HasColumnType("real");

                    b.HasKey("TaxRateId");

                    b.ToTable("TaxRate");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.PaymentMethod.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBank")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Subject.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId");

                    b.ToTable("SubjectAddresses");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Subject.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubjectId"), 1L, 1);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Dic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ico")
                        .HasColumnType("int");

                    b.Property<bool>("IsVatPayer")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubjectId");

                    b.HasIndex("AddressId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.User.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastLoginTmstmp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisteredTmpstmp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.BasicInvoice", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.Document.Document");

                    b.ToTable("BasicInvoices", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.CorrectiveTaxDocument", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.Document.Document");

                    b.ToTable("CorrectiveTaxDocuments", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.InvoiceTemplate", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.Document.Document");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("InvoiceTemplates", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.ProformaInvoice", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.Document.Document");

                    b.ToTable("proformaInvoices", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.RegularInvoice", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.Document.Document");

                    b.Property<string>("EndCondition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("RepeatPeriod")
                        .HasColumnType("int");

                    b.ToTable("RegularInvoices", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.PaymentMethod.BankDetail", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.PaymentMethod.PaymentMethod");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Iban")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Swift")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("BankDetails", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Subject.Customer", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.Subject.Subject");

                    b.Property<bool>("AresUpdateAllowed")
                        .HasColumnType("bit");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.User.Admin", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.User.User");

                    b.ToTable("Administrators", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.User.Purser", b =>
                {
                    b.HasBaseType("TEJ0017_FakturacniSystem.Models.User.User");

                    b.ToTable("Pursers", (string)null);
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.Document", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.PaymentMethod.BankDetail", "BankDetail")
                        .WithMany()
                        .HasForeignKey("BankDetailPaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TEJ0017_FakturacniSystem.Models.Subject.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerSubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TEJ0017_FakturacniSystem.Models.PaymentMethod.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TEJ0017_FakturacniSystem.Models.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankDetail");

                    b.Navigation("Customer");

                    b.Navigation("PaymentMethod");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentItem", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.Document.Document", "Document")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TEJ0017_FakturacniSystem.Models.Document.TaxRate", "TaxRate")
                        .WithMany()
                        .HasForeignKey("TaxRateId");

                    b.Navigation("Document");

                    b.Navigation("TaxRate");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Subject.Subject", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.Subject.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.BasicInvoice", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.Document.Document", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.BasicInvoice", "DocumentId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.CorrectiveTaxDocument", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.Document.Document", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.CorrectiveTaxDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.InvoiceTemplate", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.Document.Document", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.InvoiceTemplate", "DocumentId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.ProformaInvoice", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.Document.Document", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.ProformaInvoice", "DocumentId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.RegularInvoice", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.Document.Document", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.Document.DocumentTypes.RegularInvoice", "DocumentId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.PaymentMethod.BankDetail", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.PaymentMethod.PaymentMethod", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.PaymentMethod.BankDetail", "PaymentMethodId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Subject.Customer", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.Subject.Subject", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.Subject.Customer", "SubjectId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.User.Admin", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.User.User", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.User.Admin", "UserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.User.Purser", b =>
                {
                    b.HasOne("TEJ0017_FakturacniSystem.Models.User.User", null)
                        .WithOne()
                        .HasForeignKey("TEJ0017_FakturacniSystem.Models.User.Purser", "UserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TEJ0017_FakturacniSystem.Models.Document.Document", b =>
                {
                    b.Navigation("InvoiceItems");
                });
#pragma warning restore 612, 618
        }
    }
}