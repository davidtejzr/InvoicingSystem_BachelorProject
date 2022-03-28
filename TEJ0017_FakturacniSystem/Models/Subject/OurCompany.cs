namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class OurCompany : Subject
    {
        public string? WebPage { get; set; }
        public string? HeaderDesc { get; set; }
        public string? FooterDesc { get; set; }
        public int? DueInterval { get; set; }
        public  int? DocumentNumberLength { get; set; }
        public string? DefaultMJ { get; set; }
        public int? DefaultVat { get; set; }
        public Dictionary<string, string> NumSeries { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailText { get; set; }

        private static OurCompany _instance = null;
        private OurCompany()
        {
            NumSeries = new Dictionary<string, string>();
        }

        public void fillAllData(int ico, string dic, string name, bool isVatPayer, string email, string telephone, Address address, string webPage, string headerDesc, 
            string footerDesc, int dueInterval, int documentNumberLength, Dictionary<string, string> numSeries, string defaultMJ, int defaultVat, string emailSubject, string emailText)
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
            this.DocumentNumberLength = documentNumberLength;
            this.NumSeries = numSeries;
            this.DefaultMJ = defaultMJ;
            this.DefaultVat = defaultVat;
            this.EmailSubject = emailSubject;
            this.EmailText = emailText;
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

        public void fillDocSetData(string headerDesc, string footerDesc, int dueInterval, int documentNumberLength, string defaultMJ, int defaultVat)
        {
            this.HeaderDesc = headerDesc;
            this.FooterDesc = footerDesc;
            this.DueInterval = dueInterval;
            this.DocumentNumberLength = documentNumberLength;
            this.DefaultMJ = defaultMJ;
            this.DefaultVat = defaultVat;
        }

        public void fillEmailData(string emailSubject, string emailText)
        {
            this.EmailSubject = emailSubject;
            this.EmailText = emailText;
        }

        public static OurCompany getInstance()
        {
            if(_instance == null)
                _instance = new OurCompany();

            return _instance;
        }

        

    }
}
