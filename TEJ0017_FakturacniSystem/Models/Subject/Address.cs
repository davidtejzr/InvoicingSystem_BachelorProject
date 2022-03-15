namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class Address
    {
        public int AddressId { get; private set; }
        public string Street { get; private set; }
        public string HouseNumber { get; private set; }
        public string City { get; private set; }
        public string Zip { get; private set; }
        public string State { get; private set; }

        public Address() { }

        public Address(string street, string houseNumber, string city, string zip, string state)
        {
            this.Street = street;
            this.HouseNumber = houseNumber;
            this.City = city;
            this.Zip = zip;
            this.State = state;
        }

    }
}
