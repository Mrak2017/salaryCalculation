using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalaryCalculation.Models
{
    public class SalaryCalculationDBContext : DbContext
    {
        public SalaryCalculationDBContext (DbContextOptions<SalaryCalculationDBContext> options): base(options)
        {
        }

        public DbSet<Person> Person { get; set; }
    }
}
