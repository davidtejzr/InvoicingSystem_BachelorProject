#nullable disable
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TEJ0017_FakturacniSystem.Models;
using TEJ0017_FakturacniSystem.Models.Document;
using TEJ0017_FakturacniSystem.Models.Document.DocumentTypes;
using TEJ0017_FakturacniSystem.Models.PaymentMethod;
using TEJ0017_FakturacniSystem.Models.Subject;

namespace TEJ0017_FakturacniSystem.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationContext _context;

        public DocumentsController(ApplicationContext context)
        {
            _context = context;
        }

        public static string RenderViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            controller.ViewData["OurCompany"] = ourCompany;

            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw, new HtmlHelperOptions());
                viewResult.View.RenderAsync(viewContext);

                return sw.GetStringBuilder().ToString();
            }
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var documents = await _context.Documents.Include(c => c.Customer).ToListAsync();
            return View(documents);
        }

        // GET: Documents/BasicInvoiceDetail/5
        public async Task<FileResult> BasicInvoiceDetail(int? id)
        {
            if (id == null)
                return null;

            var document = await _context.BasicInvoices.Include(c => c.Customer).Include(u => u.User).Include(ca => ca.Customer.Address).
                Include(b => b.BankDetail).Include(pm => pm.PaymentMethod).Include(di => di.DocumentItems).FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return null;
            }

            HtmlToPdfConvertor htmlToPdfConvertor = new HtmlToPdfConvertor();
            string outputHtml = RenderViewToString(this, "BasicInvoiceDetail", document);
            MemoryStream output = htmlToPdfConvertor.getDocumentPdf(outputHtml);
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

            basicInvoice.DocumentItems = documentItems;
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
                && basicInvoice.User != null && basicInvoice.DocumentItems != null)
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

        public ContentResult CustomerData(string customerName)
        {
            Customer customer = _context.Customers.Include("Address").FirstOrDefault(m => m.Name == customerName);
            Dictionary<string, string> customerData = new Dictionary<string, string>();
            customerData.Add("customerStreet", customer.Address.Street);
            customerData.Add("customerHouseNumber", customer.Address.HouseNumber);
            customerData.Add("customerCity", customer.Address.City);
            customerData.Add("customerZip", customer.Address.Zip);
            customerData.Add("customerIco", customer.Ico.ToString());
            customerData.Add("customerDic", customer.Dic);

            string jsonResult = JsonConvert.SerializeObject(customerData);
            return Content(jsonResult);
        }

        public ContentResult IsBankMethod(string paymentMethodName)
        {
            PaymentMethod paymentMethod = _context.PaymentMethods.FirstOrDefault(m => m.Name == paymentMethodName);
            if(paymentMethod.IsBank)
                return Content("true");
            else
                return Content("false");
        }
    }
}