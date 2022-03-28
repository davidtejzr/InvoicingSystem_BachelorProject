using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Document
{
    public class Item
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Název položky je povinný.")]
        [Display(Name = "Název položky")]
        public string Name { get; set; }

        [Display(Name = "Cena bez DPH")]
        public float? PriceWoVat { get; set; }

        [Display(Name = "Sazba DPH")]
        public int? Vat { get; set; }

        [Display(Name = "Cena s DPH")]
        public float Price { get; set; }

        [Display(Name = "Výchozí jednotka")]
        public string? defaultUnit { get; set; }

        [Display(Name = "Popis")]
        public string? Description { get; set; }
    }
}
