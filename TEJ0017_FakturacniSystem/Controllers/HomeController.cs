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

        public HomeController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();

            return View(ourCompany);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection user, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var searchedUser = _context.Users.Where(u => u.Login.Equals(user["Login"])).FirstOrDefault();
                if ((searchedUser != null) && Crypto.VerifyHashedPassword(searchedUser.Password, user["Password"]))
                {
                    var claims = new List<Claim>
                    {
                        new Claim("user", searchedUser.Login),
                        new Claim("role", searchedUser.GetType().ToString()),
                        new Claim("userName", searchedUser.Name + " " + searchedUser.Surname)
                    };

                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));
                    //HttpContext.Session.SetString("UserName", searchedUser.Name + " " + searchedUser.Surname);

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.message = "Špatné uživatelské jméno nebo heslo!";
                }
            }
            
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult err403()
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