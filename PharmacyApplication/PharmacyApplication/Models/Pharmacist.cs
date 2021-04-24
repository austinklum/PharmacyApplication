using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class Pharmacist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [RegularExpression(@"[a-zA-Z ]{1,50}", ErrorMessage = "Name must be 1-50 letters long with only letters and spaces")]
        public string Name { get; set; }
        public string Pronouns { get; set; }
    }
}
