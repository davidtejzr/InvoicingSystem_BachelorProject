using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.InvoiceItem
{
    public abstract class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public string Name { get; set; }
        public float UnitPrice { get; set; }
        public float Amount { get; set; }
        public string? Unit { get; set; }
    }
}
