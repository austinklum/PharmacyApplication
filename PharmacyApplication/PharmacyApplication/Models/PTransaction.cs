using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class PTransaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int HolderId { get; set; }
    }
}
