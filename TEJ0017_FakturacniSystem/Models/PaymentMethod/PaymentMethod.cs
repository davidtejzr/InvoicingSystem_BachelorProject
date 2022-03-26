using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.PaymentMethod
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Název platební metody je povinný.")]
        [Display(Name = "Název platební metody")]
        public string Name { get; set; }

        [Display(Name = "Popis")]
        public string? Description { get; set; }

        [Display(Name = "Bankovní metoda")]
        public bool IsBank { get; set; }

        public bool IsVisible { get; set; }
    }
}
