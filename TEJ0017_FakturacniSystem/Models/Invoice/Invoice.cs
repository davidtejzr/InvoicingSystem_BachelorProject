using TEJ0017_FakturacniSystem.Models;

namespace TEJ0017_FakturacniSystem.Models.Invoice
{
    public abstract class Invoice
    {
        public int InvoiceId { get; set; }
        public User.Purser User { get; set; }
        public Subject.Customer Customer { get; set; }
        public List<InvoiceItem.InvoiceItem> InvoiceItems { get; set; }
        public PaymentMethod.PaymentMethod PaymentMethod { get; set; }
        public string? VariableSymbol { get; set; }
        public string? ConstantSymbol { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public float? Discount { get; set; }
        public bool IsPaid { get; set; }
    }
}
