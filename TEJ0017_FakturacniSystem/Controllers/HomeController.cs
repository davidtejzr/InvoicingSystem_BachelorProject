using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TEJ0017_FakturacniSystem.Models;

namespace TEJ0017_FakturacniSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            /*TESTING FEATURES*/
            AresCommunicator aresCommunicator = new AresCommunicator();
            Dictionary<string, string> data = aresCommunicator.getInfoBySubjectName("ASSECO a.s.");

            if (int.Parse(data["Ares_odpovedi.Odpoved.Pocet_zaznamu"]) == -1)
            {
                Console.Error.WriteLine("result -1 !!!");
            }
            else if ( int.Parse(data["Ares_odpovedi.Odpoved.Pocet_zaznamu"]) == 0)
            {
                Console.WriteLine("Nenalezeny zadne odpovidajici zaznamy v ARESu");
            }
            else if(int.Parse(data["Ares_odpovedi.Odpoved.Pocet_zaznamu"]) == 1)
            {
                Console.WriteLine(data["Ares_odpovedi.Odpoved.Zaznam.Obchodni_firma"]);
            }
            else if(int.Parse(data["Ares_odpovedi.Odpoved.Pocet_zaznamu"]) > 1)
            {
                foreach (KeyValuePair<string, string> keyValuePair in data)
                {
                    if (keyValuePair.Key.Contains("Ares_odpovedi.Odpoved.Zaznam.Obchodni_firma"))
                        Console.WriteLine(keyValuePair.Value);
                }
            }
            else
            {
                Console.Error.WriteLine("Unexpected value on key 'Pocet_zaznamu'");
            }

            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            return View(ourCompany);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}