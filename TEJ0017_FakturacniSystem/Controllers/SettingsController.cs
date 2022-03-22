using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.Subject;

namespace TEJ0017_FakturacniSystem.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ApplicationContext _context;

        public SettingsController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult OurCompanyEdit()
        {
            OurCompany ourCompany = OurCompany.getInstance();
            return View(ourCompany);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OurCompanyEdit(SubjectNotMapped subject)
        {
            OurCompany ourCompany = OurCompany.getInstance();

            if (ModelState.IsValid)
            {
                //_context.Add(ourCompany);
                //_context.SaveChangesAsync();
                ViewBag.SuccessMessage = "Změný úspěšně uloženy.";
                return View(ourCompany);
            }

            ViewBag.ErrorMessage = "Formulář obsahuje chyby, změny nelze uložit!";
            return View(ourCompany);
        }

        public IActionResult DocumentSettings()
        {
            return View();
        }

        public IActionResult EmailTemplate()
        {
            return View();
        }
    }
}
