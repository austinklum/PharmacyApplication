using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        [DisplayName("Physician Name")]
        public string PhysicianName { get; set; }
        [DisplayName("Physician License Number")]
        public string PhysicianLicenseNumber { get; set; }
        [DisplayName("Patient Name")]
        public string PatientName { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Patient Date of Birth")]
        public DateTime PatientDOB { get; set; }
        [DisplayName("Patient Address")]
        public string PatientAddress { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Issued on")]
        public DateTime IssuedDate { get; set; }
        [DisplayName("Physician Verification Status")]
        public bool? PhysicianVerified { get; set; }
        [DisplayName("Patient Verification Status")]
        public bool? PatientVerified { get; set; }
        public bool? SentToInsurance { get; set; }
        [DisplayName("Bill Creation Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? BillCreated { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double SubtotalCost { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double TaxCost { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double TotalCost { get; set; }
    }
}
