#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.PaymentMethod;

namespace TEJ0017_FakturacniSystem.Controllers.Settings
{
    [Authorize]
    public class PaymentMethodsController : Controller
    {
        private readonly ApplicationContext _context;

        public PaymentMethodsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: PaymentMethods
        public async Task<IActionResult> Index()
        {
            var bankDetails = await _context.BankDetails.Where(bd => bd.IsVisible == true).ToListAsync();
            var paymentMethods = await _context.PaymentMethods.Where(pm => pm.IsVisible == true).ToListAsync();
            var paymentMethodsOnly = paymentMethods.Except(bankDetails);
            return View(paymentMethodsOnly);
        }

        // GET: PaymentMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                if (_context.PaymentMethods.Where(pm => pm.IsVisible == true).FirstOrDefault(pm => pm.Name == paymentMethod.Name) == null)
                {
                    _context.Add(paymentMethod);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Platební metoda vytvořena.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["ErrorMessage"] = "Platební metoda s tímto názvem již existuje!";
                    return View(paymentMethod);
                }
            }

            ViewData["ErrorMessage"] = "Chyba validace! Opravte prosím chybně vyplněné údaje.";
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.PaymentMethodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                PaymentMethod pm = _context.PaymentMethods.Where(pm => pm.IsVisible == true).FirstOrDefault(pm => (pm.Name == paymentMethod.Name) && (pm.PaymentMethodId != id));
                if ((pm != null) && (pm.PaymentMethodId != id))
                {
                    ViewData["ErrorMessage"] = "Platební metoda s tímto názvem již existuje!";
                    return View(paymentMethod);
                }

                try
                {
                    _context.Update(paymentMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentMethodExists(paymentMethod.PaymentMethodId))
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
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            paymentMethod.IsVisible = false;
            _context.Update(paymentMethod);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Platební metoda smazána.";
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentMethodExists(int id)
        {
            return _context.PaymentMethods.Any(e => e.PaymentMethodId == id);
        }
    }
}
