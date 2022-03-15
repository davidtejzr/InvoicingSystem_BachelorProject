namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class OurCompany : Subject
    {
        public string? WebPage { get; private set; }
        private static OurCompany _instance = null;
        private OurCompany()
        {
            /*this.Ico = 12345;
            this.Dic = "CZ12345";
            this.Name = "Firma s.r.o.";
            this.IsVatPayer = true;
            this.Email = "firma@firma.cz";
            this.Telephone = "+420 123 456 789";
            //this.Address = new Address();
            this.WebPage = "https://www.firma.cz/";*/
        }

        public void fillData(int ico, string dic, string name, bool isVatPayer, string email, string telephone, string webPage)
        {
            this.Ico = ico;
            this.Dic = dic;
            this.Name = name;
            this.IsVatPayer = isVatPayer;
            this.Email = email;
            this.Telephone = telephone;
            //this.Address = new Address();
            this.WebPage = webPage;
        }

        public static OurCompany getInstance()
        {
            if(_instance == null)
                _instance = new OurCompany();

            return _instance;
        }

        

    }
}
