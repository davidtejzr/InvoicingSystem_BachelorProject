using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEJ0017_FakturacniSystem.Models.User
{
    public abstract class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Login je vyžadován!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Heslo je vyžadováno!")]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Ověření hesla je vyžadováno!")]
        [Compare("Password", ErrorMessage = "Zadaná hesla se neshodují!")]
        [Display(Name = "Heslo")]
        public string PasswordVerify { get; set; }

        [Required(ErrorMessage = "Jméno je vyžadováno!")]
        [Display(Name = "Jméno")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Příjmení je vyžadováno!")]
        [Display(Name = "Příjmení")]
        public string Surname { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        public string? Telephone { get; set; }

        [Display(Name = "Poslední přihlášení")]
        public DateTime LastLoginTmstmp { get; set; }

        [Display(Name = "Registrace")]
        public DateTime RegisteredTmpstmp { get; set; }
    }
}
