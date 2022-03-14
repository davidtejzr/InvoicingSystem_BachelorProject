namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class Customer : Subject
    {
        public bool AresUpdateAllowed { get; private set; }
        public string ContactName { get; private set; }
        public string ContactSurname { get; private set; }
    }
}
