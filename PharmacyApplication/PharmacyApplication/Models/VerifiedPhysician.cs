using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class VerifiedPhysician
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("License Number")]
        public string LicenseNumber { get; set; }
        public string Pronouns { get; set; }
    }
}
