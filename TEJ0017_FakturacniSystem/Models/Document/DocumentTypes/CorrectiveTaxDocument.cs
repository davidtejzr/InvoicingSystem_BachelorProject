using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Document.DocumentTypes
{
    public class CorrectiveTaxDocument : Document
    {
        [Display(Name = "Důvod opravy")]
        public string CorrectionReason { get; set; }
    }
}
