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
            return View(await _context.BankDetails.ToListAsync());
        }

        // GET: BankDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankDetail = await _context.BankDetails
                .FirstOrDefaultAsync(m => m.PaymentMethodId == id);
            if (bankDetail == null)
            {
                return NotFound();
            }

            return View(bankDetail);
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
                _context.Add(bankDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "Chyba validace! Opravte prosím chybně vyplněné údaje.";
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(Index));
            }
            return View(bankDetail);
        }

        // GET: BankDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankDetail = await _context.BankDetails
                .FirstOrDefaultAsync(m => m.PaymentMethodId == id);
            if (bankDetail == null)
            {
                return NotFound();
            }

            return View(bankDetail);
        }

        // POST: BankDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankDetail = await _context.BankDetails.FindAsync(id);
            _context.BankDetails.Remove(bankDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankDetailExists(int id)
        {
            return _context.BankDetails.Any(e => e.PaymentMethodId == id);
        }
    }
}
