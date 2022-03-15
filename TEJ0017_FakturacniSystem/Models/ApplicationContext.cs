using Microsoft.EntityFrameworkCore;
using TEJ0017_FakturacniSystem.Models.InvoiceItem;
using TEJ0017_FakturacniSystem.Models.Invoice;
using TEJ0017_FakturacniSystem.Models.Subject;

namespace TEJ0017_FakturacniSystem.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<User.User> Users { get; set; }
        public DbSet<PaymentMethod.PaymentMethod> PaymentMethods { get; set; }
        public DbSet<InvoiceItem.InvoiceItem> InvoiceItems { get; set; }
        public DbSet<InvoiceItem.TaxRate> InvoiceItemsTaxRates { get; set; }
        public DbSet<Subject.Subject> Subjects { get; set; }
        public DbSet<Subject.Address> SubjectAddresses { get; set; }
        public DbSet<Invoice.Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User.Admin>().ToTable("Administrators");
            builder.Entity<User.Purser>().ToTable("Pursers");

            builder.Entity<Subject.Customer>().ToTable("Customers");

            builder.Entity<InvoiceItem.InvoiceItemVat>().ToTable("VatInvoiceItems");
            builder.Entity<InvoiceItem.InvoiceItemNoVat>().ToTable("NoVatInvoiceItems");

            builder.Entity<Invoice.InvoiceVat>().ToTable("VatInvoices");
            builder.Entity<Invoice.InvoiceNoVat>().ToTable("NoVatInvoices");
        }

        public DbSet<TEJ0017_FakturacniSystem.Models.InvoiceItem.InvoiceItemVat> InvoiceItemVat { get; set; }

        public DbSet<TEJ0017_FakturacniSystem.Models.Invoice.InvoiceVat> InvoiceVat { get; set; }

        public DbSet<TEJ0017_FakturacniSystem.Models.Invoice.InvoiceNoVat> InvoiceNoVat { get; set; }

        public DbSet<TEJ0017_FakturacniSystem.Models.Subject.Customer> Customer { get; set; }
    }
}
