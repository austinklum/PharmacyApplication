using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Patient DOB")]
        public DateTime PatientDOB { get; set; }
        [DisplayName("Patient Address")]
        public string PatientAddress { get; set; }
        [DisplayName("Issued on")]
        public DateTime IssuedDate { get; set; }
        [DisplayName("Physician Verification Status")]
        public bool? PhysicianVerified { get; set; }
        [DisplayName("Patient Verification Status")]
        public bool? PatientVerified { get; set; }
        [DisplayName("Bill Status")]
        public bool BillCreated { get; set; }
    }
}
