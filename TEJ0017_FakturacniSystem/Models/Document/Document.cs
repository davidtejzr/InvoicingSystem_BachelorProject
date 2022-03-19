using System.ComponentModel.DataAnnotations;
using TEJ0017_FakturacniSystem.Models;

namespace TEJ0017_FakturacniSystem.Models.Document
{
    public abstract class Document
    {
        public int DocumentId { get; set; }
        public User.User User { get; set; }
        public Subject.Customer Customer { get; set; }
        public ICollection<DocumentItem> InvoiceItems { get; set; }
        public PaymentMethod.PaymentMethod PaymentMethod { get; set; }
        public PaymentMethod.BankDetail BankDetail { get; set; }

        [Display(Name = "Variabilní symbol")]
        public string? VariableSymbol { get; set; }

        [Display(Name = "Konstantní symbol")]
        public string? ConstantSymbol { get; set; }

        [Display(Name = "Datum vystavení")]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Datum splatnosti")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Datum zdanitelného plnění")]
        public DateTime? TaxDate { get; set; }

        [Display(Name = "Sleva")]
        public float? Discount { get; set; }

        [Display(Name = "Celková částka")]
        public float TotalAmount { get; set; }

        [Display(Name = "Uhrazeno")]
        public bool IsPaid { get; set; }

        [Display(Name = "Popisek hlavičky dokladu")]
        public string? headerDescription { get; set; }

        [Display(Name = "Popisek patičky dokladu")]
        public string? footerDescription { get; set; }
    }
}
