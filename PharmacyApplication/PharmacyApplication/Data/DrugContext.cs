using Microsoft.EntityFrameworkCore;
using PharmacyApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Data
{
    public class DrugContext : DbContext
    {
        public DrugContext(DbContextOptions<DrugContext> options) : base(options)
        {
        }

        public DbSet<Drug> Drugs { get; set; }
    }
}
