using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class Subtransaction
    {
        public int Id { get; set; }
        public int PTransactionId { get; set; }
        public int DrugId { get; set; }
        public int Count { get; set; }
        public double AmountPaid { get; set; }
        public int Accepted { get; set; }
    }
}
