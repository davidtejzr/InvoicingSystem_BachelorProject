namespace TEJ0017_FakturacniSystem.Models.PaymentMethod
{
    public class BankDetail : PaymentMethod
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string Swift { get; set; }
        public string Iban { get; set; }
    }
}
