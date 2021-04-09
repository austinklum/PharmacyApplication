using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class Drug
    {
        public int Id { get; set; }
        public string MedicalName { get; set; }
        public string Type { get; set; }
        public int Stock { get; set; }
        public double CostPer { get; set; }
        public string RecommendedDose { get; set; }
    }
}
