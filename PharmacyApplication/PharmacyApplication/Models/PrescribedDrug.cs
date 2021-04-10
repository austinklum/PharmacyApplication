using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [NotMapped]
        public string DrugName { get; set; }
    }
}
