namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class OurCompany : Subject
    {
        public string? WebPage { get; set; }
        public string? HeaderDesc { get; set; }
        public string? FooterDesc { get; set; }
        public int? DueInterval { get; set; }
        private static OurCompany _instance = null;
        private OurCompany() { }

        public void fillAllData(int ico, string dic, string name, bool isVatPayer, string email, string telephone, Address address, string webPage, string headerDesc, 
            string footerDesc, int dueInterval)
        {
            this.Ico = ico;
            this.Dic = dic;
            this.Name = name;
            this.IsVatPayer = isVatPayer;
            this.Email = email;
            this.Telephone = telephone;
            this.Address = address;
            this.WebPage = webPage;
            this.HeaderDesc = headerDesc;
            this.FooterDesc = footerDesc;
            this.DueInterval = dueInterval;
        }

        public void fillCompanyData(int ico, string dic, string name, bool isVatPayer, string email, string telephone, Address address, string webPage)
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

        public void fillDocSetData(string headerDesc, string footerDesc, int dueInterval)
        {
            this.HeaderDesc = headerDesc;
            this.FooterDesc = footerDesc;
            this.DueInterval = dueInterval;
        }

        public static OurCompany getInstance()
        {
            if(_instance == null)
                _instance = new OurCompany();

            return _instance;
        }

        

    }
}
