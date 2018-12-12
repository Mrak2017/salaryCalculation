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

        public DbSet<Person> Persons { get; set; }

        public DbSet<Person2Group> Person2Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(p => p.InsertDate)
                .HasDefaultValueSql("date('now')");

            modelBuilder.Entity<Person>()
                .Property(p => p.UpdateDate)
                .HasDefaultValueSql("date('now')");

            modelBuilder.Entity<Person>()
                .Property(p => p.Active)
                .HasDefaultValue(true);

            modelBuilder.Entity<Person2Group>()
                .Property(p => p.InsertDate)
                .HasDefaultValueSql("date('now')");

            modelBuilder.Entity<Person2Group>()
                .Property(p => p.UpdateDate)
                .HasDefaultValueSql("date('now')");

            modelBuilder.Entity<Person2Group>()
                .Property(p => p.Active)
                .HasDefaultValue(true);
        }
    }
}
