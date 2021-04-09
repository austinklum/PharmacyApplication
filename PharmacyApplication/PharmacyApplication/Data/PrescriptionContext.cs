using Microsoft.EntityFrameworkCore;
using PharmacyApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Data
{
    public class PrescriptionContext : DbContext
    {
        public PrescriptionContext(DbContextOptions<PrescriptionContext> options) : base(options)
        {
        }

        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescribedDrug> PrescribedDrugs { get; set; }
    }
}
