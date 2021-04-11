using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class VerifiedPatient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}
