using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Xml.Linq;

namespace TEJ0017_FakturacniSystem
{
    public class AresCommunicator
    {
        private string xmlResult;
        private XmlDocument xmlDocument;
        private XmlDocument xmlDocumentDph;
        public Dictionary<string, string> parsedData;
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string aresUrl = "https://wwwinfo.mfcr.cz/cgi-bin/ares/darv_std.cgi?";
        private static readonly string aresUrlDph = "http://wwwinfo.mfcr.cz/cgi-bin/ares/darv_bas.cgi?";

        public AresCommunicator()
        {
            xmlResult = String.Empty;
            parsedData = new Dictionary<string, string>();
            xmlDocument = new XmlDocument();
            xmlDocumentDph = new XmlDocument();
        }

        //parser ARES dat podle ICO - pro lepsi praci s daty bylo XML prevedno na JSON se kterym dale pracuji
        private void parseDataByIco(string jsonText, string jsonTextDph)
        {
            dynamic jsonDataRaw = JObject.Parse(jsonText);
            dynamic jsonDataRawDph = JObject.Parse(jsonTextDph);

            if (jsonDataRaw["are:Ares_odpovedi"]["are:Odpoved"]["are:Pocet_zaznamu"] == "1")
            {
                //ico
                dynamic jsonDataRecord = jsonDataRaw["are:Ares_odpovedi"]["are:Odpoved"]["are:Zaznam"];
                dynamic jsonDataAddress = jsonDataRecord["are:Identifikace"]["are:Adresa_ARES"];

                string subjectName = jsonDataRecord["are:Obchodni_firma"];
                string subjectStreet = jsonDataAddress["dtt:Nazev_ulice"];
                string subjectHouseNumber = jsonDataAddress["dtt:Cislo_domovni"];
                string subjectCity = jsonDataAddress["dtt:Nazev_obce"];
                string subjectZip = jsonDataAddress["dtt:PSC"];

                parsedData.Add("SubjectName", subjectName);
                parsedData.Add("SubjectStreet", subjectStreet);
                parsedData.Add("SubjectHouseNumber", subjectHouseNumber);
                parsedData.Add("SubjectCity", subjectCity);
                parsedData.Add("SubjectZip", subjectZip);

                //dic
                try
                {
                    string subjectDic = jsonDataRawDph["are:Ares_odpovedi"]["are:Odpoved"]["D:VBAS"]["D:DIC"]["#text"];
                    if (subjectDic != null)
                        parsedData.Add("SubjectDic", subjectDic);
                }
                catch (Exception ex) { }

                parsedData.Add("InfoMsg", "Data úšpěšně stažena z databáze ARES.");
            }
            else
            {
                parsedData.Add("ErrorMsg", "Subjekt nenalezen.");
            }
        }

        //XML parser dat nazvu subjektu z ARESu
        private void parseDataBySubjectName()
        {
            XDocument data = XDocument.Parse(xmlResult);
            Dictionary<string, string> dictionaryXmlData = new Dictionary<string, string>();

            foreach (XElement element in data.Descendants().Where(x => x.HasElements == false))
            {
                int keyInt = 0;
                string keyName = element.Name.LocalName;

                var parent = element.Parent;
                while (parent != null)
                {
                    keyName = parent.Name.LocalName + "." + keyName;
                    parent = parent.Parent;
                }

                while (dictionaryXmlData.ContainsKey(keyName))
                {
                    keyName = keyName + "_" + keyInt++;
                }

                dictionaryXmlData.Add(keyName, element.Value);
            }

            //pri zadani obecneho zaznamu je vracena hodnota '-1', ARES vyhrozuje pri opakovanem vraceni tohoto vysledku zablokovanim adresy IP - nutno osetrit
            //zatim osetreno vyhledavanim od 3+ zadanych znaku + pridana odezva na stisk klavesy 500 ms (logika v 'aresSearchFunctions.js')
            if (int.Parse(dictionaryXmlData["Ares_odpovedi.Odpoved.Pocet_zaznamu"]) == -1)
            {
                Console.Error.WriteLine("result -1 !!!");
            }
            else if (int.Parse(dictionaryXmlData["Ares_odpovedi.Odpoved.Pocet_zaznamu"]) == 0)
            {
                Console.WriteLine("Nenalezeny zadne odpovidajici zaznamy v ARESu");
            }
            else if (int.Parse(dictionaryXmlData["Ares_odpovedi.Odpoved.Pocet_zaznamu"]) == 1)
            {
                string subjectName = dictionaryXmlData["Ares_odpovedi.Odpoved.Zaznam.Obchodni_firma"];
                string ico = dictionaryXmlData["Ares_odpovedi.Odpoved.Zaznam.ICO"];
                parsedData.Add(subjectName, ico);
            }
            else if (int.Parse(dictionaryXmlData["Ares_odpovedi.Odpoved.Pocet_zaznamu"]) > 1)
            {
                string subjectName = string.Empty;
                string ico = string.Empty;
                foreach (KeyValuePair<string, string> keyValuePair in dictionaryXmlData)
                {
                    if (keyValuePair.Key.Contains("Ares_odpovedi.Odpoved.Zaznam.Obchodni_firma"))
                    {
                        subjectName = keyValuePair.Value;       
                    }
                    else if (keyValuePair.Key.Contains("Ares_odpovedi.Odpoved.Zaznam.ICO"))
                    {
                        ico = keyValuePair.Value;
                    }

                    if((subjectName.Length != 0) && (ico.Length != 0))
                    {
                        parsedData.Add(subjectName, ico);

                        subjectName = string.Empty;
                        ico = string.Empty;
                    }
                }
            }
            else
            {
                Console.Error.WriteLine("Unexpected value on key 'Pocet_zaznamu'");
            }

        }

        //nacteni vsech dat z ARESU odpovidajici zadanemu udaji ICO
        public Dictionary<string, string> getInfoByIco(string ico)
        {
            //dotaz na sluzbu 'Standard' - ICO udaje
            //https://wwwinfo.mfcr.cz/cgi-bin/ares/darv_std.cgi?ico=27074358
            var result = httpClient.GetStringAsync(aresUrl + "ico=" + ico);
            //dotaz na sluzbu 'Basic' - obsahuje DIC udaj
            //http://wwwinfo.mfcr.cz/cgi-bin/ares/darv_bas.cgi?ico=27074358
            var resultDph = httpClient.GetStringAsync(aresUrlDph + "ico=" + ico);

            xmlDocument.LoadXml(result.Result);
            xmlDocumentDph.LoadXml(resultDph.Result);
            parseDataByIco(JsonConvert.SerializeXmlNode(xmlDocument), JsonConvert.SerializeXmlNode(xmlDocumentDph));

            return parsedData;
        }

        //nacteni vsech vyhledanych subjektu z ARESu (budou pouzity pouze nazvy subjektu, dalsi data se nactou pomoci ICO - jednodussi varianta nez prochazet jejich slozite XML) 
        public Dictionary<string, string> getInfoBySubjectName(string subjectName)
        {
            subjectName = subjectName.Replace(" ", "%20");
            var result = httpClient.GetStringAsync(aresUrl + "obchodni_firma=" + subjectName);
            xmlResult = result.Result;
            parseDataBySubjectName();

            return parsedData;
        }

    }
}
