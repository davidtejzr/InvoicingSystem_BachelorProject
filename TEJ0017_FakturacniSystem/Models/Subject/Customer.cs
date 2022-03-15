using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class Customer : Subject
    {
        public bool AresUpdateAllowed { get; private set; }

        [Required]
        public string ContactName { get; private set; }

        [Required]
        public string ContactSurname { get; private set; }

        public Customer() { }

        public Customer(int ico, string dic, string name, bool isVatPayer, string? email, string? telephone, Address address, bool aresUpdateAllowed, string contactName, string contactSurname)
        {
            this.Ico = ico;
            this.Dic = dic;
            this.Name = name;
            this.IsVatPayer = isVatPayer;
            this.Email = email;
            this.Telephone = telephone;
            this.Address = address;
            this.AresUpdateAllowed = aresUpdateAllowed;
            this.ContactName = contactName;
            this.ContactSurname = contactSurname;
        }
    }
}
