using Microsoft.EntityFrameworkCore;
using PharmacyApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Data
{
    public class PharmacistContext : DbContext
    {
        public PharmacistContext(DbContextOptions<PharmacistContext> options) : base(options)
        {
        }

        public DbSet<Pharmacist> Pharmacists { get; set; }
    }
}
