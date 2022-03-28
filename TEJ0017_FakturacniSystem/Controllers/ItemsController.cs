#nullable disable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.Document;

namespace TEJ0017_FakturacniSystem.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationContext _context;

        public ItemsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            ViewData["OurCompany"] = ourCompany;

            return View(await _context.Items.ToListAsync());
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            ViewData["OurCompany"] = ourCompany;

            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            ViewData["OurCompany"] = ourCompany;

            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Položka uložena.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ErrorMessage"] = "Chyba validace! Doplňte prosím chybějící údaje.";
            return View(item);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            ViewData["OurCompany"] = ourCompany;

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,PriceWoVat,Vat,Price,DefaultUnit,Description")] Item item)
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            ViewData["OurCompany"] = ourCompany;

            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
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

            ViewData["ErrorMessage"] = "Chyba validace! Doplňte prosím chybějící údaje.";
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kontakt smazán.";
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
