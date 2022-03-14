using TEJ0017_FakturacniSystem.Models;

namespace TEJ0017_FakturacniSystem.Models.Invoice
{
    public abstract class Invoice
    {
        public int InvoiceId { get; protected set; }
        public User.Purser User { get; protected set; }
        public Subject.Customer Customer { get; protected set; }
        public List<InvoiceItem.InvoiceItem> InvoiceItems { get; protected set; }

    }
}
