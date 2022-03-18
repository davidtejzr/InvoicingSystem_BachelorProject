namespace TEJ0017_FakturacniSystem.Models.Document
{
    public class TaxRate
    {
        public int TaxRateId { get; set; }
        public string Name { get; set; }
        public float Rate { get; set; }
        public string? GoodsTypes { get; set; }
    }
}
