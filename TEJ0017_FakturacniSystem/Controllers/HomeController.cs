using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Web.Helpers;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.User;

namespace TEJ0017_FakturacniSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public HomeController(ApplicationContext context)
        {
            _context = context;
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

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection user)
        {
            if(ModelState.IsValid)
            {
                var searchedUser = _context.Users.Where(u => u.Login.Equals(user["Login"])).FirstOrDefault();
                if ((searchedUser != null) && Crypto.VerifyHashedPassword(searchedUser.Password, user["Password"]))
                {
                    var claims = new List<Claim>
                    {
                        new Claim("user", searchedUser.Login),
                        new Claim("role", searchedUser.GetType().ToString())
                    };

                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));
                    //HttpContext.Session.SetString("Login", searchedUser.Login);
                    return RedirectToAction(nameof(Index));
                    return Redirect("/Home/Index");
                }
                else
                {
                    return Unauthorized();
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
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