namespace TEJ0017_FakturacniSystem.Models.PaymentMethod
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; protected set; }
        public string Name { get; protected set; }
        public string? Description { get; protected set; }
    }
}
