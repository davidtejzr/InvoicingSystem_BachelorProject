using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public abstract class Subject
    {
        public int SubjectId { get; protected set; }

        [Required]
        public int Ico { get; protected set; }

        public string? Dic { get; protected set; }

        [Required]
        public string Name { get; protected set; }

        public bool IsVatPayer { get; protected set; }

        public string? Email { get; protected set; }

        public string? Telephone { get; protected set; }

        public Address Address { get; protected set; }

        public int AddressId { get; protected set; }
    }
}
