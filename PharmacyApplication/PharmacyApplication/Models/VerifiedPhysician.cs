using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class VerifiedPhysician
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("License Number")]
        [RegularExpression(@"[A-Z][A-Z]-[0-9][0-9][0-9][0-9][0-9][0-9]", ErrorMessage = "License number must be in form AA-DDDDDD where AA is a state prefix")]
        public string LicenseNumber { get; set; }
        public string Pronouns { get; set; }
    }
}
