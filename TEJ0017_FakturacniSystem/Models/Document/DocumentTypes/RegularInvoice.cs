namespace TEJ0017_FakturacniSystem.Models.Document.DocumentTypes
{
    public class RegularInvoice : Document
    {
        public bool IsActive { get; set; }
        public int RepeatPeriod { get; set; }
        public string EndCondition { get; set; }
    }
}
