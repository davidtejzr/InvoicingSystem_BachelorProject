using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public abstract class Subject
    {
        public int SubjectId { get; set; }
        
        [Required(ErrorMessage = "Identifikátor subjektu je povinný.")]
        [Display(Name = "IČO")]
        public int Ico { get; set; }

        [Display(Name = "DIČ")]
        public string? Dic { get; set; }

        [Required(ErrorMessage = "Název subjektu je povinný.")]
        [Display(Name = "Název subjektu")]
        public string Name { get; set; }

        [Display(Name = "Plátce DPH")]
        public bool IsVatPayer { get; set; }

        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Display(Name = "Telefon")]
        public string? Telephone { get; set; }

        public Address Address { get; set; }

        public int AddressId { get; set; }
    }
}
