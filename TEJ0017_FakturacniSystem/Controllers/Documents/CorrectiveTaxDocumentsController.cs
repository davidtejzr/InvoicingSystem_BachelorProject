#nullable disable
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class CorrectiveTaxDocumentsController : Controller
    {
        private readonly ApplicationContext _context;

        public CorrectiveTaxDocumentsController(ApplicationContext context)
        {
            _context = context;
        }

        public static string RenderViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            OurCompany ourCompany = OurCompany.getInstance();
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

        // GET: Index
        public async Task<IActionResult> Index(string? filterRadioPaid, string? filterMinDatetime, string? filterMaxDatetime, string? filterCustomerSelect, int? pageNumber)
        {
            ViewData["OurCompany"] = OurCompany.getInstance();
            List<string> customers = _context.Customers.Where(c => c.IsVisible == true).Select(c => c.Name).Distinct().ToList();
            ViewData["Customers"] = customers;
            var documents = _context.CorrectiveTaxDocuments.ToList();

            if ((filterRadioPaid != null) && (filterMinDatetime != null) && (filterMaxDatetime != null) && (filterCustomerSelect != null))
            {
                DateTime minDateTime = Convert.ToDateTime(filterMinDatetime);
                DateTime maxDateTime = Convert.ToDateTime(filterMaxDatetime);

                documents = await _context.CorrectiveTaxDocuments.Include(c => c.Customer).OrderByDescending(d => d.IssueDate).Where(d => (d.IssueDate >= minDateTime) && (d.IssueDate <= maxDateTime)).ToListAsync();
                if (filterRadioPaid == "paid")
                {
                    documents = await _context.CorrectiveTaxDocuments.Include(c => c.Customer).OrderByDescending(d => d.IssueDate).Where(d => (d.IssueDate >= minDateTime) && (d.IssueDate <= maxDateTime) && d.IsPaid).ToListAsync();
                }
                else if (filterRadioPaid == "unpaid")
                {
                    documents = await _context.CorrectiveTaxDocuments.Include(c => c.Customer).OrderByDescending(d => d.IssueDate).Where(d => (d.IssueDate >= minDateTime) && (d.IssueDate <= maxDateTime) && !d.IsPaid).ToListAsync();
                }

                if (filterCustomerSelect != "Nevybráno")
                {
                    documents = documents.Where(c => c.Customer.Name == filterCustomerSelect).ToList();
                }
            }
            else
            {
                documents = await _context.CorrectiveTaxDocuments.Include(c => c.Customer).OrderByDescending(d => d.IssueDate).ToListAsync();

                if (documents.Count > 0)
                    ViewData["FirstInvoice"] = _context.CorrectiveTaxDocuments.ToList().Min(d => d.IssueDate);
            }

            int pageSize = 10;
            return View(await PaginatedList<Document>.Create(documents.AsQueryable().AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Documents/Detail/5
        public async Task<FileResult> Detail(int? id)
        {
            if (id == null)
                return null;

            var document = await _context.CorrectiveTaxDocuments.Include(c => c.Customer).Include(u => u.User).Include(ca => ca.Customer.Address).
                Include(b => b.BankDetail).Include(pm => pm.PaymentMethod).Include(di => di.DocumentItems).FirstOrDefaultAsync(d => d.DocumentId == id);
            if (document == null)
            {
                return null;
            }

            string outputHtml = RenderViewToString(this, "Detail", document);
            HtmlToPdfConvertor htmlToPdfConvertor = new HtmlToPdfConvertor(outputHtml);
            MemoryStream output = htmlToPdfConvertor.getDocumentPdf();
            output.Position = 0;

            return File(output, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }

        // GET: Documents/Create
        public async Task<IActionResult> Create()
        {
            OurCompany ourCompany = OurCompany.getInstance();
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
        public IActionResult Create(CorrectiveTaxDocument correctiveTaxDocument, IFormCollection itemsValues)
        {
            //inicializace nactenych dat pro zpetne generovani
            OurCompany ourCompany = OurCompany.getInstance();
            var bankDetails =  _context.BankDetails.Where(bd => bd.IsVisible == true).ToList();
            var paymentMethods =  _context.PaymentMethods.Where(pm => pm.IsVisible == true).ToList();
            var paymentMethodsOnly = paymentMethods.Except(bankDetails);
            ViewData["Customers"] =  _context.Customers.Where(c => c.IsVisible == true).ToList();
            ViewData["PaymentMethods"] = paymentMethodsOnly;
            ViewData["BankDetails"] = bankDetails;
            ViewData["OurCompany"] = ourCompany;
            ViewData["NextNum"] = correctiveTaxDocument.DocumentNo;

            //nastaveni platce/neplatce DPH k dokumentu pro pozdejsi otevreni
            if (ourCompany.IsVatPayer)
                correctiveTaxDocument.IsWithVat = true;
            else
                correctiveTaxDocument.IsWithVat = false;

            //zpracovani polozek dokumentu
            float sum = 0;
            List<DocumentItem> documentItems = new List<DocumentItem>();
            var itemsNames = itemsValues["ItemName"];
            var itemsPrices = itemsValues["ItemPrice"];
            var itemsAmounts = itemsValues["ItemAmount"];
            var itemsUnits = itemsValues["ItemUnit"];
            var itemsVats = itemsValues["ItemVat"];

            for (int i = 0; i < itemsNames.Count; i++)
            {
                DocumentItem documentItem = new DocumentItem();
                documentItem.Name = itemsNames[i];
                documentItem.UnitPrice = float.Parse(itemsPrices[i]);
                documentItem.Amount = float.Parse(itemsAmounts[i]);
                documentItem.Unit = itemsUnits[i];
                if (ourCompany.IsVatPayer)
                {
                    documentItem.Vat = int.Parse(itemsVats[i]);
                    sum += documentItem.UnitPrice * documentItem.Amount * ((float)documentItem.Vat / 100 + 1);
                }
                else
                {
                    sum += documentItem.UnitPrice * documentItem.Amount;
                }
                documentItems.Add(documentItem);
            }

            //prirazeni listu zpracovanych polozek ke tride document
            correctiveTaxDocument.DocumentItems = documentItems;

            //zpracovani rucne zadaneho zakaznika
            if(itemsValues["customCustomerAddressSwitch"] == "1")
            {
                Address customAddress = new Address();
                customAddress.Street = itemsValues["CustomStreet"];
                customAddress.HouseNumber = itemsValues["CustomHouseNumber"];
                customAddress.City = itemsValues["CustomCity"];
                customAddress.Zip = itemsValues["CustomZip"];

                Customer customCustomer = _context.Customers.FirstOrDefault(m => m.Name == itemsValues["Customer"].ToString());
                customCustomer.Name = itemsValues["CustomSubName"];
                customCustomer.Address = customAddress;

                if (itemsValues["CustomIco"] != "")
                    customCustomer.Ico = int.Parse(itemsValues["CustomIco"]);
                else
                    customCustomer.Ico = 0;

                if (itemsValues["CustomDic"] != "")
                    customCustomer.Dic = itemsValues["CustomDic"];

                correctiveTaxDocument.Customer = customCustomer;
                ViewData["IsCustomAddress"] = "1";
            }
            else
            {
                correctiveTaxDocument.Customer = _context.Customers.FirstOrDefault(m => m.Name == itemsValues["Customer"].ToString());
            }

            //prirazeni prihlaseneho uzivatele k dokumentu
            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            string userLogin = identity.Claims.FirstOrDefault(c => c.Type == "user").Value.ToString();
            correctiveTaxDocument.User = _context.Users.FirstOrDefault(m => m.Login == userLogin);

            //prirazeni dalsich udaju
            correctiveTaxDocument.PaymentMethod = _context.PaymentMethods.FirstOrDefault(m => m.Name == itemsValues["PaymentMethod"].ToString());
            correctiveTaxDocument.BankDetail = _context.BankDetails.FirstOrDefault(m => m.Name == itemsValues["BankDetail"].ToString());
            correctiveTaxDocument.IsPaid = false;
            correctiveTaxDocument.IssueDate = DateTime.Now;

            //prirazeni vychozi hlavicky, paticky pokud nebyla vyplnena
            if (correctiveTaxDocument.headerDescription == null)
                correctiveTaxDocument.headerDescription = ourCompany.HeaderDesc;
            if(correctiveTaxDocument.footerDescription == null)
                correctiveTaxDocument.footerDescription = ourCompany.FooterDesc;

            //vypocet celkove castky (vcetne pripradne slevy)
            float calcDiscountAmount = (float)-(sum * (correctiveTaxDocument.Discount / 100));
            correctiveTaxDocument.TotalAmount = (float?)Math.Round(sum + calcDiscountAmount, 2);

            //kontrola validity a zapis dokumentu
            if (ModelState.IsValid && correctiveTaxDocument.Customer != null && correctiveTaxDocument.PaymentMethod != null && correctiveTaxDocument.User != null 
                && correctiveTaxDocument.DocumentItems != null)
            {
                //kontrola duplicity dokumentu
                if (_context.Documents.FirstOrDefault(d => d.DocumentNo == correctiveTaxDocument.DocumentNo) != null)
                {
                    ViewData["ErrorMessage"] = "Faktura s tímto číslem již existuje!";
                    ViewData["BasicInvoice"] = correctiveTaxDocument;
                    return View(correctiveTaxDocument);
                }

                _context.Add(correctiveTaxDocument);
                _context.SaveChanges();

                //nastaveni ukazatele generovani ciselne rady na aktualni hodnotu
                NumericalSeriesGenerator numericalSeriesGenerator = new NumericalSeriesGenerator();
                numericalSeriesGenerator.saveChanges();
                DataInitializer.getInstance().updateOurCompanyDataInJson();

                TempData["SuccessMessage"] = "Faktura úspěšně vystavena.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["BasicInvoice"] = correctiveTaxDocument;
            ViewData["ErrorMessage"] = "Chyba validity formuláře!";
            return View(correctiveTaxDocument);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var correctiveTaxDocument = await _context.Documents.Include(pm => pm.PaymentMethod).Include(bd => bd.BankDetail).Include(di => di.DocumentItems).Include(c => c.Customer).FirstOrDefaultAsync(d => d.DocumentId == id);
            if (correctiveTaxDocument == null)
            {
                return NotFound();
            }

            OurCompany ourCompany = OurCompany.getInstance();
            var bankDetails = await _context.BankDetails.Where(bd => bd.IsVisible == true).ToListAsync();
            var paymentMethods = await _context.PaymentMethods.Where(pm => pm.IsVisible == true).ToListAsync();
            var paymentMethodsOnly = paymentMethods.Except(bankDetails);

            ViewData["Customers"] = await _context.Customers.Where(c => c.IsVisible == true).ToListAsync();
            ViewData["PaymentMethods"] = paymentMethodsOnly;
            ViewData["BankDetails"] = bankDetails;
            ViewData["OurCompany"] = ourCompany;

            return View(correctiveTaxDocument);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CorrectiveTaxDocument correctiveTaxDocument, IFormCollection itemsValues)
        {
            if (id != correctiveTaxDocument.DocumentId)
            {
                return NotFound();
            }

            //inicializace nactenych dat pro zpetne generovani
            OurCompany ourCompany = OurCompany.getInstance();
            var bankDetails = _context.BankDetails.Where(bd => bd.IsVisible == true).ToList();
            var paymentMethods = _context.PaymentMethods.Where(pm => pm.IsVisible == true).ToList();
            var paymentMethodsOnly = paymentMethods.Except(bankDetails);
            ViewData["Customers"] = _context.Customers.Where(c => c.IsVisible == true).ToList();
            ViewData["PaymentMethods"] = paymentMethodsOnly;
            ViewData["BankDetails"] = bankDetails;
            ViewData["OurCompany"] = ourCompany;

            //zpracovani polozek dokumentu
            float sum = 0;
            List<DocumentItem> documentItems = new List<DocumentItem>();
            var itemsNames = itemsValues["ItemName"];
            var itemsPrices = itemsValues["ItemPrice"];
            var itemsAmounts = itemsValues["ItemAmount"];
            var itemsUnits = itemsValues["ItemUnit"];
            var itemsVats = itemsValues["ItemVat"];

            for (int i = 0; i < itemsNames.Count; i++)
            {
                DocumentItem documentItem = new DocumentItem();
                documentItem.Name = itemsNames[i];
                documentItem.UnitPrice = float.Parse(itemsPrices[i]);
                documentItem.Amount = float.Parse(itemsAmounts[i]);
                documentItem.Unit = itemsUnits[i];
                if (correctiveTaxDocument.IsWithVat)
                {
                    documentItem.Vat = int.Parse(itemsVats[i]);
                    sum += documentItem.UnitPrice * documentItem.Amount * ((float)documentItem.Vat / 100 + 1);
                }
                else
                {
                    sum += documentItem.UnitPrice * documentItem.Amount;
                }
                documentItems.Add(documentItem);
            }

            //prirazeni listu zpracovanych polozek ke tride document
            correctiveTaxDocument.DocumentItems = documentItems;

            //zpracovani rucne zadaneho zakaznika
            if (itemsValues["customCustomerAddressSwitch"] == "1")
            {
                Address customAddress = new Address();
                customAddress.Street = itemsValues["CustomStreet"];
                customAddress.HouseNumber = itemsValues["CustomHouseNumber"];
                customAddress.City = itemsValues["CustomCity"];
                customAddress.Zip = itemsValues["CustomZip"];

                Customer customCustomer = new Customer();
                customCustomer.Name = itemsValues["CustomSubName"];
                customCustomer.Address = customAddress;

                if (itemsValues["CustomIco"] != "")
                    customCustomer.Ico = int.Parse(itemsValues["CustomIco"]);
                else
                    customCustomer.Ico = 0;

                if (itemsValues["CustomDic"] != "")
                    customCustomer.Dic = itemsValues["CustomDic"];

                correctiveTaxDocument.Customer = customCustomer;
                ViewData["IsCustomAddress"] = "1";
            }
            else
            {
                correctiveTaxDocument.Customer = _context.Customers.FirstOrDefault(m => m.Name == itemsValues["Customer"].ToString());
            }

            //prirazeni prihlaseneho uzivatele k dokumentu
            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            string userLogin = identity.Claims.FirstOrDefault(c => c.Type == "user").Value.ToString();
            correctiveTaxDocument.User = _context.Users.FirstOrDefault(m => m.Login == userLogin);

            //prirazeni dalsich udaju
            correctiveTaxDocument.PaymentMethod = _context.PaymentMethods.FirstOrDefault(m => m.Name == itemsValues["PaymentMethod"].ToString());
            correctiveTaxDocument.BankDetail = _context.BankDetails.FirstOrDefault(m => m.Name == itemsValues["BankDetail"].ToString());

            //vypocet celkove castky (vcetne pripradne slevy)
            float calcDiscountAmount = (float)-(sum * (correctiveTaxDocument.Discount / 100));
            correctiveTaxDocument.TotalAmount = (float?)Math.Round(sum + calcDiscountAmount, 2);

            //kontrola validity a zapis dokumentu
            if (ModelState.IsValid && correctiveTaxDocument.Customer != null && correctiveTaxDocument.PaymentMethod != null && correctiveTaxDocument.User != null
                && correctiveTaxDocument.DocumentItems != null)
            {
                //kontrola duplicity dokumentu (mimo aktualne upravovany)
                if (_context.Documents.FirstOrDefault(d => (d.DocumentNo == correctiveTaxDocument.DocumentNo) && (d.DocumentId != correctiveTaxDocument.DocumentId)) != null)
                {
                    ViewData["ErrorMessage"] = "Faktura s tímto číslem již existuje!";
                    ViewData["BasicInvoice"] = correctiveTaxDocument;
                    return View(correctiveTaxDocument);
                }

               //odstraneni puvodnich polozek faktury
               var oldItems = _context.DocumentItems.Where(di => di.Document.DocumentId == correctiveTaxDocument.DocumentId);
                _context.DocumentItems.RemoveRange(oldItems);

                _context.Update(correctiveTaxDocument);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Změny na faktuře " + correctiveTaxDocument.DocumentNo + " uloženy.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["BasicInvoice"] = correctiveTaxDocument;
            ViewData["ErrorMessage"] = "Chyba validity formuláře!";
            return View(correctiveTaxDocument);
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
            Customer customer = _context.Customers.Include("Address").FirstOrDefault(m => m.Name == customerName);
            Dictionary<string, string> customerData = new Dictionary<string, string>();

            if (customer != null)
            {
                customerData.Add("customerStreet", customer.Address.Street);
                customerData.Add("customerHouseNumber", customer.Address.HouseNumber);
                customerData.Add("customerCity", customer.Address.City);
                customerData.Add("customerZip", customer.Address.Zip);
                customerData.Add("customerIco", customer.Ico.ToString());
                customerData.Add("customerDic", customer.Dic);
            }

            string jsonResult = JsonConvert.SerializeObject(customerData);
            return Content(jsonResult);
        }

        public ContentResult IsBankMethod(string paymentMethodName)
        {
            PaymentMethod paymentMethod = _context.PaymentMethods.FirstOrDefault(m => m.Name == paymentMethodName);
            if((paymentMethod != null) && paymentMethod.IsBank)
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

        public ContentResult PriceListItems(string searchString)
        {
            var items = _context.Items.Where(i => i.Name.Contains(searchString)).ToList();
            string jsonResult = JsonConvert.SerializeObject(items);

            return Content(jsonResult);
        }

        // POST: Documents/SendEmail/5
        [HttpPost, ActionName("SendEmail")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(int id, IFormCollection values)
        {
            if (id == null)
                return null;

            var document = await _context.Documents.Include(c => c.Customer).Include(u => u.User).Include(ca => ca.Customer.Address).
                Include(b => b.BankDetail).Include(pm => pm.PaymentMethod).Include(di => di.DocumentItems).FirstOrDefaultAsync(d => d.DocumentId == id);
            if (document == null)
            {
                return null;
            }

            string outputHtml = RenderViewToString(this, "Detail", document);
            HtmlToPdfConvertor htmlToPdfConvertor = new HtmlToPdfConvertor(outputHtml);
            MemoryStream output = htmlToPdfConvertor.getDocumentPdf();
            output.Position = 0;

            string subjectText = values["emailSubject"] + " " + document.DocumentNo;
            string fileName = "faktura_" + document.DocumentNo;
            EmailSender emailSender = new EmailSender(values["emailReceiver"], subjectText, values["emailText"], output, fileName);
            bool returnState = emailSender.SendEmail();

            if(returnState)
                TempData["SuccessMessage"] = "Email úspěšně odeslán.";
            else
                TempData["ErrorMessage"] = "Došlo k chybě při odesílání emailu.";

            return RedirectToAction(nameof(Index));
        }
    }
}