using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.PaymentMethod
{
    public class BankDetail : PaymentMethod
    {
        [Required(ErrorMessage = "Název banky je povinný.")]
        [Display(Name = "Název banky")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Číslo účtu je povinné.")]
        [Display(Name = "Číslo účtu")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Kód banky je povinný.")]
        [Display(Name = "Kód banky")]
        public string BankCode { get; set; }

        [Display(Name = "SWIFT")]
        public string? Swift { get; set; }

        [Display(Name = "IBAN")]
        public string? Iban { get; set; }
    }
}
