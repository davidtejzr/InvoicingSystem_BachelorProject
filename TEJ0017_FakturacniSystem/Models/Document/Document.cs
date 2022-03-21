using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEJ0017_FakturacniSystem.Models;

namespace TEJ0017_FakturacniSystem.Models.Document
{
    public abstract class Document
    {
        public int DocumentId { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        [ValidateNever]
        public User.User User { get; set; }

        public int? CustomerId { get; set; }
        [ValidateNever]
        public Subject.Customer Customer { get; set; }

        [ValidateNever]
        public ICollection<DocumentItem> DocumentItems { get; set; }

        public int? PaymentmethodId { get; set; }
        [ValidateNever]
        public PaymentMethod.PaymentMethod PaymentMethod { get; set; }

        public int? BankDetailId { get; set; }
        [ValidateNever]
        public PaymentMethod.BankDetail? BankDetail { get; set; }

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
