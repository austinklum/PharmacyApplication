using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class PrescriptionDetailsViewModel
    {
        public Prescription CurrentPrescription { get; set; }
        public List<PrescribedDrug> PrescribedDrugs { get; set; }
    }
}
