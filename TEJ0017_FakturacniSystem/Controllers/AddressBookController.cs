#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.Subject;

namespace TEJ0017_FakturacniSystem.Controllers
{
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
            return View(await _context.Customer.ToListAsync());
        }

        // GET: AddressBook/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.SubjectId == id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.C:\Users\david\AppData\Local\Temp\Temp1_projekt.zip\tej0017-vis\web\VIS-rezervacniSystem\Models\REZERVACE.cs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection values)
        {
            int ico;
            Int32.TryParse(values["Ico"], out ico);

            Address address = new Address(values["Address.Street"], values["Address.HouseNumber"], values["Address.City"], values["Address.Zip"], values["Address.State"]);
            Customer customer = new Customer(ico, values["Dic"], values["Name"], bool.Parse(values["IsVatPayer"]),
                values["Email"], values["Telephone"], address, bool.Parse(values["AresUpdateAllowed"]), values["ContactName"], values["ContactSurname"]);

            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }


        //proc nefunguje ???
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer, Address address)
        {
            //_context.Customer.Add(customer);
            //_context.SubjectAddresses.Add(address);

            _context.Add(customer);

            _context.SaveChanges();


            return RedirectToAction("Index");
        }*/

        // GET: AddressBook/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
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

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.SubjectId == id);
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
            var customer = await _context.Customer.FindAsync(id);
            var address = await _context.SubjectAddresses.FindAsync(customer.AddressId);
            _context.Customer.Remove(customer);
            _context.SubjectAddresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.SubjectId == id);
        }
    }
}
