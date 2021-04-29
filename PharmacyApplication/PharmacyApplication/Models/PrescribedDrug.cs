using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class PrescribedDrug
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int PrescriptionId { get; set; }
        public int Count { get; set; }
        public string Dosage { get; set; }
        [DisplayName("Refill Count")]
        public int RefillCount { get; set; }
        [DisplayName("Covered Amount")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double CoveredAmount { get; set; }
        public bool Returned { get; set; }

        [NotMapped]
        [DisplayName("Drug Name")]
        public string DrugName { get; set; }
        [NotMapped]
        public Drug CurrentDrug { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double TotalCost { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double Remaining { get; set; }
    }
}
