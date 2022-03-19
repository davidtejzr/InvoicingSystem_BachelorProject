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

            OurCompany.getInstance().fillData(Int32.Parse(this.JsonDict["Ico"]), this.JsonDict["Dic"], this.JsonDict["Name"],
                bool.Parse(this.JsonDict["IsVatPayer"]), this.JsonDict["Email"], this.JsonDict["Telephone"], address, this.JsonDict["WebPage"]);
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

            using (StreamWriter s = new StreamWriter(this.Path))
            {
                var json = JsonConvert.SerializeObject(data);
                s.Write(json);
            }
        }
    }
}
