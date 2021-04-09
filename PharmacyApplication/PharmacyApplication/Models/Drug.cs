using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class Drug
    {
        public int Id { get; set; }
        [DisplayName("Drug Name")]
        public string MedicalName { get; set; }
        public string Type { get; set; }
        public int Stock { get; set; }
        [DisplayName("Cost per dose")]
        public double CostPer { get; set; }
        [DisplayName("Recommended Dose")]
        public string RecommendedDose { get; set; }
    }
}
