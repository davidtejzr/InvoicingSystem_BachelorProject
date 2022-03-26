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
    public class BasicInvoicesController : Controller
    {
        private readonly ApplicationContext _context;

        public BasicInvoicesController(ApplicationContext context)
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

        // GET: Documents/Detail/5
        public async Task<FileResult> Detail(int? id)
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
            string outputHtml = RenderViewToString(this, "Detail", document);
            MemoryStream output = htmlToPdfConvertor.getDocumentPdf(outputHtml);
            output.Position = 0;

            return File(output, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }

        // GET: Documents/Create
        public async Task<IActionResult> Create()
        {
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            var bankDetails = await _context.BankDetails.Where(bd => bd.IsVisible == true).ToListAsync();
            var paymentMethods = await _context.PaymentMethods.Where(pm => pm.IsVisible == true).ToListAsync();
            var paymentMethodsOnly = paymentMethods.Except(bankDetails);
            ViewData["Customers"] = await _context.Customers.Where(c => c.IsVisible == true).ToListAsync();
            ViewData["PaymentMethods"] = paymentMethodsOnly;
            ViewData["BankDetails"] = bankDetails;
            ViewData["OurCompany"] = ourCompany;
            NumericalSeriesGenerator numericalSeriesGenerator = new NumericalSeriesGenerator();
            ViewData["NextNum"] = numericalSeriesGenerator.generateDocumentNumber();

            return View();
        }

        // POST: Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BasicInvoice basicInvoice, IFormCollection itemsValues)
        {
            //inicializace nactenych dat pro zpetne generovani
            Models.Subject.OurCompany ourCompany = Models.Subject.OurCompany.getInstance();
            var bankDetails =  _context.BankDetails.Where(bd => bd.IsVisible == true).ToList();
            var paymentMethods =  _context.PaymentMethods.Where(pm => pm.IsVisible == true).ToList();
            var paymentMethodsOnly = paymentMethods.Except(bankDetails);
            ViewData["Customers"] =  _context.Customers.Where(c => c.IsVisible == true).ToList();
            ViewData["PaymentMethods"] = paymentMethodsOnly;
            ViewData["BankDetails"] = bankDetails;
            ViewData["OurCompany"] = ourCompany;
            ViewData["NextNum"] = basicInvoice.DocumentNo;

            var itemsNames = itemsValues["ItemName"];
            var itemsPrices = itemsValues["ItemPrice"];
            var itemsAmounts = itemsValues["ItemAmount"];
            var itemsUnits = itemsValues["ItemUnit"];

            //vypocet celkove castky
            float sum = 0;
            List<DocumentItem> documentItems = new List<DocumentItem>();
            for(int i = 0; i < itemsNames.Count; i++)
            {
                DocumentItem documentItem = new DocumentItem();
                documentItem.Name = itemsNames[i];
                string commaChange = itemsPrices[i].Replace(".", ",");
                documentItem.UnitPrice = float.Parse(commaChange);
                documentItem.Amount = float.Parse(itemsAmounts[i]);
                documentItem.Unit = itemsUnits[i];
                sum += documentItem.UnitPrice * documentItem.Amount;

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

            string discountVal = itemsValues["DiscountVal"];
            basicInvoice.Discount = float.Parse(discountVal.Replace(".", ","));
            float calcDiscountAmount = (float)-(sum * (basicInvoice.Discount / 100));
            basicInvoice.TotalAmount = (float?)Math.Round(sum - calcDiscountAmount, 2);

            if (ModelState.IsValid && basicInvoice.Customer != null && basicInvoice.PaymentMethod != null && basicInvoice.User != null 
                && basicInvoice.DocumentItems != null)
            {
                if (_context.Documents.FirstOrDefault(d => d.DocumentNo == basicInvoice.DocumentNo) != null)
                {
                    ViewData["ErrorMessage"] = "Faktura s tímto číslem již existuje!";
                    ViewData["BasicInvoice"] = basicInvoice;
                    return View(basicInvoice);
                }

                _context.Add(basicInvoice);
                _context.SaveChanges();

                //nastaveni ukazatele generovani ciselne rady na aktualni hodnotu
                NumericalSeriesGenerator numericalSeriesGenerator = new NumericalSeriesGenerator();
                numericalSeriesGenerator.saveChanges();
                DataInitializer.getInstance().updateOurCompanyDataInJson();

                TempData["SuccessMessage"] = "Faktura úspěšně vystavena.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["BasicInvoice"] = basicInvoice;
            ViewData["ErrorMessage"] = "Chyba validity formuláře!";
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

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Faktura smazána.";
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.DocumentId == id);
        }

        public ContentResult CustomerData(string customerName)
        {
            Customer customer = _context.Customers.Include("Address").Where(c => c.IsVisible == true).FirstOrDefault(m => m.Name == customerName);
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

        public async Task<IActionResult> SetPaid(int? id)
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

            document.IsPaid = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SetUnpaid(int? id)
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

            document.IsPaid = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}