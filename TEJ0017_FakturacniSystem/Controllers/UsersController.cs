#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.User;

namespace TEJ0017_FakturacniSystem.Controllers
{
    [Authorize(Roles = "TEJ0017_FakturacniSystem.Models.User.Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationContext _context;

        public UsersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Admin admin, Purser purser, IFormCollection values)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.FirstOrDefault(u => u.Login == values["Login"].ToString()) == null)
                {
                    if (values["AccountType"] == "admin")
                    {
                        admin.RegisteredTmpstmp = DateTime.Now;
                        admin.LastLoginTmstmp = DateTime.Now;
                        admin.Password = Crypto.HashPassword(admin.Password);
                        _context.Add(admin);
                    }
                    else if (values["AccountType"] == "purser")
                    {
                        purser.RegisteredTmpstmp = DateTime.Now;
                        purser.LastLoginTmstmp = DateTime.Now;
                        purser.Password = Crypto.HashPassword(purser.Password);
                        _context.Add(purser);
                    }
                    else
                        return BadRequest();

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Uživatel úspěšně vytvořen.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["ErrorMessage"] = "Uživatel s tímto názvem již existuje!";
                    return View();
                }
            }

            ViewData["ErrorMessage"] = "Chyba validace! Opravte prosím chybně vyplněné údaje.";
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.GetType() == typeof(Admin))
                ViewBag.UserType = "Administrátor";
            else if (user.GetType() == typeof(Purser))
                ViewBag.UserType = "Účetní";

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Admin admin, Purser purser, IFormCollection values)
        {
            if ((id != admin.UserId) && (id != purser.UserId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //todo - zakazat aktualizaci datumu
                    if (values["AccountType"] == "Administrátor")
                    {
                        //admin.RegisteredTmpstmp = DateTime.Now;
                        //admin.LastLoginTmstmp = DateTime.Now;
                        admin.Password = Crypto.HashPassword(admin.Password);
                        _context.Update(admin);
                    }
                    else if (values["AccountType"] == "Účetní")
                    {
                        //purser.RegisteredTmpstmp = DateTime.Now;
                        //purser.LastLoginTmstmp = DateTime.Now;
                        purser.Password = Crypto.HashPassword(purser.Password);
                        _context.Update(purser);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(admin.UserId) && !UserExists(purser.UserId))
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

            var user = await _context.Users.FindAsync(id);
            if (user.GetType() == typeof(Admin))
                ViewBag.UserType = "Administrátor";
            else if (user.GetType() == typeof(Purser))
                ViewBag.UserType = "Účetní";

            ViewData["ErrorMessage"] = "Chyba validace! Opravte prosím chybně vyplněné údaje.";
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
