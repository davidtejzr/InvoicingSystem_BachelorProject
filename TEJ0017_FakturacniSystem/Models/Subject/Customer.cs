using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class Customer : Subject
    {
        public bool AresUpdateAllowed { get; set; }

        [Required(ErrorMessage = "Jméno kontaktní osoby je povinné.")]
        [Display(Name = "Jméno")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Příjmení kontaktní osoby je povinné.")]
        [Display(Name = "Příjmení")]
        public string ContactSurname { get; set; }
    }
}
