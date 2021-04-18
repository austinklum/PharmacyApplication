using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class PrescriptionsViewModel
    {
        public List<Prescription> Prescriptions { get; set; }
        public bool IncludeProcessed { get; set; }
    }
}
