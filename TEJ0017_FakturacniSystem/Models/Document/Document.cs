using TEJ0017_FakturacniSystem.Models;

namespace TEJ0017_FakturacniSystem.Models.Document
{
    public abstract class Document
    {
        public int DocumentId { get; set; }
        public User.Purser User { get; set; }
        public Subject.Customer Customer { get; set; }
        public ICollection<DocumentItem> InvoiceItems { get; set; }
        public PaymentMethod.PaymentMethod PaymentMethod { get; set; }
        public string? VariableSymbol { get; set; }
        public string? ConstantSymbol { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? TaxDate { get; set; }
        public float? Discount { get; set; }
        public bool IsPaid { get; set; }
        public string? headerDescription { get; set; }
        public string? footerDescription { get; set; }
    }
}
