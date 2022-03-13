using System.Xml.Linq;

namespace TEJ0017_FakturacniSystem
{
    public class AresCommunicator
    {
        private string xmlResult;
        public Dictionary<string, string> parsedDict;
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string aresUrl = "https://wwwinfo.mfcr.cz/cgi-bin/ares/darv_std.cgi?";

        public AresCommunicator()
        {
            xmlResult = String.Empty;
            parsedDict = new Dictionary<string, string>();
        }

        private void parseData()
        {
            XDocument data = XDocument.Parse(xmlResult);

            foreach (XElement element in data.Descendants().Where(x => x.HasElements == false))
            {
                int keyInt = 0;
                string keyName = element.Name.LocalName;

                var parent = element.Parent;
                while(parent != null)
                {
                    keyName = parent.Name.LocalName + "." + keyName;
                    parent = parent.Parent;
                }

                while(parsedDict.ContainsKey(keyName))
                {
                    keyName = keyName + "_" + keyInt++;
                }

                parsedDict.Add(keyName, element.Value);
            }
        }

        public Dictionary<string, string> getInfoByIco(string ico)
        {
            var result = httpClient.GetStringAsync(aresUrl + "ico=" + ico);
            xmlResult = result.Result;
            parseData();

            return parsedDict;
        }

        public Dictionary<string, string> getInfoBySubjectName(string subjectName)
        {
            subjectName = subjectName.Replace(" ", "%20");
            var result = httpClient.GetStringAsync(aresUrl + "obchodni_firma=" + subjectName);
            xmlResult= result.Result;
            parseData();

            return parsedDict;
        }

    }
}
