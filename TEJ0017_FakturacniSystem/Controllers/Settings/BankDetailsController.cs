#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.PaymentMethod;

namespace TEJ0017_FakturacniSystem.Controllers.Settings
{
    public class BankDetailsController : Controller
    {
        private readonly ApplicationContext _context;

        public BankDetailsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: BankDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.BankDetails.Where(bd => bd.IsVisible == true).ToListAsync());
        }

        // GET: BankDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BankDetail bankDetail)
        {
            if (ModelState.IsValid)
            {
                if (_context.BankDetails.Where(bd => bd.IsVisible == true).FirstOrDefault(bd => bd.Name == bankDetail.Name) == null)
                {
                    _context.Add(bankDetail);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Bankovní účet vytvořen.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["ErrorMessage"] = "Bankovní účet s tímto názvem již existuje!";
                    return View(bankDetail);
                }
            }
            ViewData["ErrorMessage"] = "Chyba validace! Opravte prosím chybně vyplněné údaje.";
            return View(bankDetail);
        }

        // GET: BankDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankDetail = await _context.BankDetails.FindAsync(id);
            if (bankDetail == null)
            {
                return NotFound();
            }
            return View(bankDetail);
        }

        // POST: BankDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BankDetail bankDetail)
        {
            if (id != bankDetail.PaymentMethodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                BankDetail bd = _context.BankDetails.Where(bd => bd.IsVisible == true).FirstOrDefault(bd => (bd.Name == bankDetail.Name) && (bd.PaymentMethodId != id));
                if((bd != null) && (bd.PaymentMethodId != id))
                {
                    ViewData["ErrorMessage"] = "Bankovní účet s tímto názvem již existuje!";
                    return View(bankDetail);
                }

                try
                {
                    _context.Update(bankDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankDetailExists(bankDetail.PaymentMethodId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["SuccessMessage"] = "Změny uloženy.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ErrorMessage"] = "Chyba validace! Opravte prosím chybně vyplněné údaje.";
            return View(bankDetail);
        }

        // POST: BankDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankDetail = await _context.BankDetails.FindAsync(id);
            bankDetail.IsVisible = false;
            _context.Update(bankDetail);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Bankovní účet smazán.";
            return RedirectToAction(nameof(Index));
        }

        private bool BankDetailExists(int id)
        {
            return _context.BankDetails.Any(e => e.PaymentMethodId == id);
        }
    }
}
