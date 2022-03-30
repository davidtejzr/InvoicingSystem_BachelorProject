using Newtonsoft.Json;
using TEJ0017_FakturacniSystem.Models.Subject;

namespace TEJ0017_FakturacniSystem
{
    public class DataInitializer
    {
        private static DataInitializer _instance = null;
        private string Path { get; set; }
        private Dictionary<string, string>? JsonDict { get; set; }
        public DataInitializer()
        {
            this.Path = "appData.json";
            this.JsonDict = new Dictionary<string,string>();
        }

        public static DataInitializer getInstance()
        {
            if(_instance == null)
                _instance = new DataInitializer();

            return _instance;
        }

        public bool initConfigFile()
        {
            if(!File.Exists(this.Path))
                return false;

            using(StreamReader r = new StreamReader(this.Path))
            {
                string json = r.ReadToEnd();
                this.JsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            initOurCompany();
            return true;
        }

        public void initOurCompany()
        {
            Address address = new Address();
            address.Street = this.JsonDict["AddressStreet"];
            address.HouseNumber = this.JsonDict["AddressHouseNumber"];
            address.City = this.JsonDict["AddressCity"];
            address.Zip = this.JsonDict["AddressZip"];

            Dictionary<string, string> numSeries = JsonConvert.DeserializeObject<Dictionary<string, string>>(this.JsonDict["NumSeries"]);
            if(numSeries == null)
                numSeries = new Dictionary<string, string>();

            OurCompany.getInstance().fillAllData(Int32.Parse(this.JsonDict["Ico"]), this.JsonDict["Dic"], this.JsonDict["Name"],
                bool.Parse(this.JsonDict["IsVatPayer"]), this.JsonDict["Email"], this.JsonDict["Telephone"], address, this.JsonDict["WebPage"], this.JsonDict["HeaderDesc"],
                this.JsonDict["FooterDesc"], Int32.Parse(this.JsonDict["DueInterval"]), Int32.Parse(this.JsonDict["DocumentNumberLength"]), numSeries, this.JsonDict["DefaultMJ"], 
                Int32.Parse(this.JsonDict["DefaultVat"]), this.JsonDict["EmailSubject"], this.JsonDict["EmailText"], this.JsonDict["EmailSenderEmail"], this.JsonDict["EmailSenderPassword"]);
        }

        public void updateOurCompanyDataInJson()
        {
            OurCompany ourCompany = OurCompany.getInstance();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Ico", ourCompany.Ico.ToString());
            data.Add("Dic", ourCompany.Dic);
            data.Add("Name", ourCompany.Name);
            data.Add("IsVatPayer", ourCompany.IsVatPayer.ToString());
            data.Add("Email", ourCompany.Email);
            data.Add("Telephone", ourCompany.Telephone);
            data.Add("WebPage", ourCompany.WebPage);
            data.Add("AddressStreet", ourCompany.Address.Street);
            data.Add("AddressHouseNumber", ourCompany.Address.HouseNumber);
            data.Add("AddressCity", ourCompany.Address.City);
            data.Add("AddressZip", ourCompany.Address.Zip);
            data.Add("HeaderDesc", ourCompany.HeaderDesc);
            data.Add("FooterDesc", ourCompany.FooterDesc);
            data.Add("DueInterval", ourCompany.DueInterval.ToString());
            data.Add("DocumentNumberLength", ourCompany.DocumentNumberLength.ToString());
            data.Add("NumSeries", JsonConvert.SerializeObject(ourCompany.NumSeries));
            data.Add("DefaultMJ", ourCompany.DefaultMJ);
            data.Add("DefaultVat", ourCompany.DefaultVat.ToString());
            data.Add("EmailSenderEmail", ourCompany.EmailSenderEmail);
            data.Add("EmailSenderPassword", ourCompany.EmailSenderPassword);
            data.Add("EmailSubject", ourCompany.EmailSubject);
            data.Add("EmailText", ourCompany.EmailText);

            using (StreamWriter s = new StreamWriter(this.Path))
            {
                var json = JsonConvert.SerializeObject(data);
                s.Write(json);
            }
        }

        public void runEntryGuide()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Ico", "123456789");
            data.Add("Dic", "CZ123456789");
            data.Add("Name", "Firma s.r.o.");
            data.Add("IsVatPayer", "true");
            data.Add("Email", "firma@firma.cz");
            data.Add("Telephone", "+420 123 456 789");
            data.Add("WebPage", "https://www.firma.cz/");
            data.Add("AddressStreet", "Hořínůška");
            data.Add("AddressHouseNumber", "252");
            data.Add("AddressCity", "Dolní Lhota");
            data.Add("AddressZip", "747 66");
            data.Add("HeaderDesc", "");
            data.Add("FooterDesc", "");
            data.Add("DueInterval", "14");
            data.Add("DocumentNumberLength", "3");
            data.Add("NumSeries", "");
            data.Add("DefaultMJ", "kus");
            data.Add("DefaultVat", "21");
            data.Add("EmailSenderEmail", "");
            data.Add("EmailSenderPassword", "");
            data.Add("EmailSubject", "");
            data.Add("EmailText", "");

            using (StreamWriter s = new StreamWriter(this.Path))
            {
                var json = JsonConvert.SerializeObject(data);
                s.Write(json);
            }
        }
    }
}
