namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class OurCompany : Subject
    {
        public string? WebPage { get; set; }
        private static OurCompany _instance = null;
        private OurCompany() { }

        public void fillData(int ico, string dic, string name, bool isVatPayer, string email, string telephone, Address address, string webPage)
        {
            this.Ico = ico;
            this.Dic = dic;
            this.Name = name;
            this.IsVatPayer = isVatPayer;
            this.Email = email;
            this.Telephone = telephone;
            this.Address = address;
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
