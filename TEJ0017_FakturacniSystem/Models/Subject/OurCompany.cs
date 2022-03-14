namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class OurCompany : Subject
    {
        public string? WebPage { get; private set; }
        private static OurCompany _instance = null;
        private OurCompany()
        {
            this.Ico = 12345;
            this.Dic = "CZ12345";
            this.Name = "Firma s.r.o.";
            this.IsVatPayer = true;
            this.Email = "firma@firma.cz";
            this.Telephone = "+420 123 456 789";
            //this.Address = new Address();
            this.WebPage = "https://www.firma.cz/";
        }

        public static OurCompany getInstance()
        {
            if(_instance == null)
                _instance = new OurCompany();

            return _instance;
        }

        

    }
}
