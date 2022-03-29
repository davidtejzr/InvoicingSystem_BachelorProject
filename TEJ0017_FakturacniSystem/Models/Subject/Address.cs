using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Ulice je povinná.")]
        [Display(Name = "Ulice")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Číslo popisné je povinné.")]
        [Display(Name = "Číslo popisné")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Město je povinné.")]
        [Display(Name = "Město")]
        public string City { get; set; }

        [Required(ErrorMessage = "PSČ je povinné.")]
        [Display(Name = "PSČ")]
        public string Zip { get; set; }

        [Display(Name = "Stát")]
        public string? State { get; set; }

    }
}
