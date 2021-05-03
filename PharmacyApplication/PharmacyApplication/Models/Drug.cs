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
        [DisplayName("Commercial Name")]
        public string CommercialName { get; set; }
        [DisplayName("Medical Name")]
        public string MedicalName { get; set; }
        [DisplayName("Drug Code")]
        public string DrugCode { get; set; }
        public string Type { get; set; }
        public int Stock { get; set; }
        [DisplayName("Cost per dose")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double CostPer { get; set; }
        [DisplayName("Recommended Dose")]
        public string RecommendedDose { get; set; }
        public string Vendor { get; set; }
    }
}
