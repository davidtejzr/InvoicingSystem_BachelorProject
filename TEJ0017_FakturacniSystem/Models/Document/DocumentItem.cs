namespace TEJ0017_FakturacniSystem.Models.Document
{
    public class DocumentItem
    {
        public int DocumentItemId { get; set; }
        public string Name { get; set; }
        public float UnitPrice { get; set; }
        public float Amount { get; set; }
        public string? Unit { get; set; }
        public TaxRate? TaxRate { get; set; }
        public Document Document { get; set; }
    }
}
