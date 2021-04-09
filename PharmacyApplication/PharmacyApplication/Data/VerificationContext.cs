using Microsoft.EntityFrameworkCore;
using PharmacyApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Data
{
    public class VerificationContext : DbContext
    {
        public VerificationContext(DbContextOptions<VerificationContext> options) : base(options)
        {
        }

        public DbSet<VerifiedPatient> VerifiedPatients { get; set; }
        public DbSet<VerifiedPhysician> VerifiedPhysicians { get; set; }
    }
}
