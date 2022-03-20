#nullable disable
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.Document;
using TEJ0017_FakturacniSystem.Models.Document.DocumentTypes;

namespace TEJ0017_FakturacniSystem.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationContext _context;

        public DocumentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var documents = await _context.Documents.Include(c => c.Customer).ToListAsync();
            return View(documents);
        }

        // GET: Documents/ExportPdf/5
        public async Task<FileResult> ExportBasicInvoiceToPdf(int? id)
        {
            if (id == null)
                return null;

            var document = await _context.BasicInvoices.Include(c => c.Customer).Include(u => u.User).FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return null;
            }

            HtmlToPdfConvertor htmlToPdfConvertor = new HtmlToPdfConvertor();
            MemoryStream output = htmlToPdfConvertor.getBasicInvoicePdf(document);
            output.Position = 0;

            return File(output, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }

        // GET: Documents/CreateBasicInvoice
        public async Task<IActionResult> CreateBasicInvoice()
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            var bankDetails = await _context.BankDetails.ToListAsync();
            var paymentMethods = await _context.PaymentMethods.ToListAsync();
            var paymentMethodsOnly = paymentMethods.Except(bankDetails);
            ViewData["Customers"] = await _context.Customers.ToListAsync();
            ViewData["PaymentMethods"] = paymentMethodsOnly;
            ViewData["BankDetails"] = bankDetails;
            ViewData["OurCompany"] = ourCompany;

            return View();
        }

        // POST: Documents/CreateBasicInvoice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBasicInvoice(BasicInvoice basicInvoice, IFormCollection itemsValues)
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            var bankDetails =  _context.BankDetails.ToList();
            var paymentMethods =  _context.PaymentMethods.ToList();
            var paymentMethodsOnly = paymentMethods.Except(bankDetails);
            ViewData["Customers"] =  _context.Customers.ToList();
            ViewData["PaymentMethods"] = paymentMethodsOnly;
            ViewData["BankDetails"] = bankDetails;
            ViewData["OurCompany"] = ourCompany;

            var itemsNames = itemsValues["ItemName"];
            var itemsPrices = itemsValues["ItemPrice"];
            var itemsAmounts = itemsValues["ItemAmount"];
            var itemsUnits = itemsValues["ItemUnit"];

            List<DocumentItem> documentItems = new List<DocumentItem>();
            for(int i = 0; i < itemsNames.Count; i++)
            {
                DocumentItem documentItem = new DocumentItem();
                documentItem.Name = itemsNames[i];
                string commaChange = itemsPrices[i].Replace(".", ",");
                documentItem.UnitPrice = float.Parse(commaChange);
                documentItem.Amount = float.Parse(itemsAmounts[i]);
                documentItem.Unit = itemsUnits[i];

                documentItems.Add(documentItem);
            }

            basicInvoice.InvoiceItems = documentItems;
            basicInvoice.Customer = _context.Customers.FirstOrDefault(m => m.Name == itemsValues["Customer"].ToString());

            basicInvoice.PaymentMethod = _context.PaymentMethods.FirstOrDefault(m => m.Name == itemsValues["PaymentMethod"].ToString());

            basicInvoice.BankDetail = _context.BankDetails.FirstOrDefault(m => m.Name == itemsValues["BankDetail"].ToString());

            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            string userLogin = identity.Claims.FirstOrDefault(c => c.Type == "user").Value.ToString();
            basicInvoice.User = _context.Users.FirstOrDefault(m => m.Login == userLogin);

            basicInvoice.IsPaid = false;
            basicInvoice.IssueDate = DateTime.Now;

            basicInvoice.TotalAmount = 0;

            if (ModelState.IsValid && basicInvoice.Customer != null && basicInvoice.PaymentMethod != null && basicInvoice.BankDetail != null
                && basicInvoice.User != null && basicInvoice.InvoiceItems != null)
            {
                _context.Add(basicInvoice);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BasicInvoice"] = basicInvoice;
            ViewBag.ErrorMessage = "Chyba validity formuláře!";
            return View(basicInvoice);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocumentId,VariableSymbol,ConstantSymbol,IssueDate,DueDate,TaxDate,Discount,IsPaid,headerDescription,footerDescription")] Document document)
        {
            if (id != document.DocumentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocumentId))
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
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.DocumentId == id);
        }
    }
}
