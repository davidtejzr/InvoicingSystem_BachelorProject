#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.Subject;

namespace TEJ0017_FakturacniSystem.Controllers
{
    [Authorize]
    public class AddressBookController : Controller
    {
        private readonly ApplicationContext _context;

        public AddressBookController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AddressBook
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.Where(c => c.IsVisible == true).ToListAsync());
        }

        // GET: AddressBook/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.SubjectId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: AddressBook/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddressBook/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (_context.Customers.Where(c => c.IsVisible == true).FirstOrDefault(c => c.Ico == customer.Ico) == null)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Kontakt uložen.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["ErrorMessage"] = "Kontant s tímto IČO již existuje!";
                    return View(customer);
                }
            }
            ViewData["ErrorMessage"] = "Chyba validace! Doplňte prosím chybějící údaje.";
            return View(customer);
            
        }

        // GET: AddressBook/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: AddressBook/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AresUpdateAllowed,ContactName,ContactSurname,Ico,Dic,Name,IsVatPayer,Email,Telephone")] Customer customer)
        {
            if (id != customer.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.SubjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: AddressBook/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.SubjectId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: AddressBook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            var address = await _context.SubjectAddresses.FindAsync(customer.AddressId);
            _context.Customers.Remove(customer);
            _context.SubjectAddresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.SubjectId == id);
        }

        public ContentResult AresDataIco(string ico)
        {
            AresCommunicator aresCom = new AresCommunicator();
            string jsonResult = JsonConvert.SerializeObject(aresCom.getInfoByIco(ico));

            return Content(jsonResult);
        }

        public ContentResult AresDataName(string name)
        {
            AresCommunicator aresCom = new AresCommunicator();
            string jsonResult = JsonConvert.SerializeObject(aresCom.getInfoBySubjectName(name));

            return Content(jsonResult);
        }
    }
}
