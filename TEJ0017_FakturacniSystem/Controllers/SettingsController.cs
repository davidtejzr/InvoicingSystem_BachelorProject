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
            @ViewData["WebPage"] = ourCompany.WebPage;
            return View(ourCompany);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OurCompanyEdit(SubjectNotMapped subject, IFormCollection values)
        {
            OurCompany ourCompany = OurCompany.getInstance();
            @ViewData["WebPage"] = values["WebPage"];

            if (ModelState.IsValid)
            {
                Address address = new Address();
                address.Street = subject.Address.Street;
                address.HouseNumber = subject.Address.HouseNumber;
                address.City = subject.Address.City;
                address.Zip = subject.Address.Zip;

                ourCompany.fillCompanyData(subject.Ico, subject.Dic, subject.Name, subject.IsVatPayer, subject.Email, subject.Telephone, address, values["WebPage"]);
                DataInitializer.getInstance().updateOurCompanyDataInJson();

                ViewData["SuccessMessage"] = "Změný úspěšně uloženy.";
                return View(ourCompany);
            }

            ViewData["ErrorMessage"] = "Formulář obsahuje chyby, změny nelze uložit!";
            return View(ourCompany);
        }

        public IActionResult DocumentSettings()
        {
            OurCompany ourCompany = OurCompany.getInstance();
            ViewData["OurCompany"] = ourCompany;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DocumentSettings(IFormCollection values)
        {
            OurCompany ourCompany = OurCompany.getInstance();
            ourCompany.fillDocSetData(values["headerDescription"], values["footerDescription"], Int32.Parse(values["defaultDueInterval"]), Int32.Parse(values["documentNumberLength"]), values["defaultMJ"], Int32.Parse(values["defaultVat"]));
            DataInitializer.getInstance().updateOurCompanyDataInJson();
            ViewData["OurCompany"] = ourCompany;

            ViewData["SuccessMessage"] = "Změny úspěšně uloženy.";
            return View(ourCompany);
        }

        public IActionResult EmailTemplate()
        {
            return View();
        }
    }
}
