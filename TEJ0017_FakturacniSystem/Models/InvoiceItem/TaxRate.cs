namespace TEJ0017_FakturacniSystem.Models.InvoiceItem
{
    public class TaxRate
    {
        public int TaxRateId { get; private set; }
        public string Name { get; private set; }
        public float Rate { get; private set; }
        public string? GoodsTypes { get; private set; }
    }
}
