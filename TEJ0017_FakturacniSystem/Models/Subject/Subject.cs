using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public abstract class Subject
    {
        [Key]
        public int Ico { get; protected set; }
        public string? Dic { get; protected set; }
        public string Name { get; protected set; }
        public bool IsVatPayer { get; protected set; }
        public string? Email { get; protected set; }
        public string? Telephone { get; protected set; }
        public Address Address { get; protected set; }

    }
}
