namespace TEJ0017_FakturacniSystem.Models.PaymentMethod
{
    public class BankDetail : PaymentMethod
    {
        public string BankName { get; private set; }
        public string AccountNumber { get; private set; }
        public string BankCode { get; private set; }
        public string Swift { get; private set; }
        public string Iban { get; private set; }
    }
}
