namespace TEJ0017_FakturacniSystem.Models.Document
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public float? PriceWoVat { get; set; }
        public int? Vat { get; set; }
        public float Price { get; set; }
        public string? defaultUnit { get; set; }
        public string? Description { get; set; }
    }
}
