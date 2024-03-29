﻿using System.ComponentModel.DataAnnotations;

namespace TEJ0017_FakturacniSystem.Models.Subject
{
    public class Customer : Subject
    {
        [Display(Name = "Jméno")]
        public string? ContactName { get; set; }

        [Display(Name = "Příjmení")]
        public string? ContactSurname { get; set; }

        public bool IsVisible { get; set; }
    }
}
