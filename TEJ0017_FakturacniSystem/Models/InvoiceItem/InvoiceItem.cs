using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.InvoiceItem
{
    public abstract class InvoiceItem
    {
        public int InvoiceItemId { get; protected set; }
        public string Name { get; protected set; }
        public float UnitPrice { get; protected set; }
        public float Amount { get; protected set; }
        public string? Unit { get; protected set; }
    }
}
