namespace TEJ0017_FakturacniSystem.Models.InvoiceItem
{
    public class InvoiceItemVat : InvoiceItem
    {
        public TaxRate TaxRate { get; private set; }
    }
}
