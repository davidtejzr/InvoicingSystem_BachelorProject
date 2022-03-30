using Microsoft.EntityFrameworkCore;

namespace TEJ0017_FakturacniSystem.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<User.User> Users { get; set; }
        public DbSet<Subject.Address> SubjectAddresses { get; set; }
        public DbSet<Subject.Subject> Subjects { get; set; }
        public DbSet<Subject.Customer> Customers { get; set; }
        public DbSet<PaymentMethod.PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentMethod.BankDetail> BankDetails { get; set; }
        public DbSet<Document.DocumentItem> DocumentItems { get; set; }
        public DbSet<Document.Document> Documents { get; set; }
        public DbSet<Document.DocumentTypes.BasicInvoice> BasicInvoices { get; set; }
        public DbSet<Document.DocumentTypes.CorrectiveTaxDocument> CorrectiveTaxDocuments { get; set; }
        public DbSet<Document.DocumentTypes.InvoiceTemplate> InvoiceTemplates { get; set; }
        public DbSet<Document.DocumentTypes.ProformaInvoice> proformaInvoices { get; set; }
        public DbSet<Document.Item> Items { get; set; }
        public object User { get; internal set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PaymentMethod.BankDetail>().ToTable("BankDetails");

            builder.Entity<User.Admin>().ToTable("Administrators");
            builder.Entity<User.Purser>().ToTable("Pursers");

            builder.Entity<Subject.Customer>().ToTable("Customers");

            builder.Entity<Document.DocumentTypes.BasicInvoice>().ToTable("BasicInvoices");
            builder.Entity<Document.DocumentTypes.CorrectiveTaxDocument>().ToTable("CorrectiveTaxDocuments");
            builder.Entity<Document.DocumentTypes.InvoiceTemplate>().ToTable("InvoiceTemplates");
            builder.Entity<Document.DocumentTypes.ProformaInvoice>().ToTable("proformaInvoices");
        }
    }
}
