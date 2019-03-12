using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;

namespace SalaryCalculation.Models
{
    public class SalaryCalculationDBContext : DbContext
    {
        public SalaryCalculationDBContext(DbContextOptions<SalaryCalculationDBContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Person2Group> Person2Groups { get; set; }

        public DbSet<OrganizationStructureItem> OrganizationStructure { get; set; }

        public DbSet<Configuration> Configs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.InitPerson(modelBuilder);
            this.InitPerson2Group(modelBuilder);
            this.InitOrganizationStructureItem(modelBuilder);
            this.InitConfigs(modelBuilder);
        }

        private void InitPerson(ModelBuilder builder)
        {
            /// Base fields
            builder.Entity<Person>()
                .Property(p => p.InsertDate)
                .HasDefaultValueSql("datetime('now')");

            builder.Entity<Person>()
                .Property(p => p.UpdateDate)
                .HasDefaultValueSql("datetime('now')");

            builder.Entity<Person>()
                .Property(p => p.Active)
                .HasDefaultValue(true);

            /// Person 2 OrganizationStructureItem

            builder.Entity<Person>()
                .HasOne(p => p.OrgStructure)
                .WithOne(o => o.Person)
                .HasForeignKey<OrganizationStructureItem>(o => o.PersonId);

            /// Indexes
            builder.Entity<Person>()
                .HasIndex(p => p.Login)
                .IsUnique();
        }

        private void InitPerson2Group(ModelBuilder builder)
        {
            /// Base fields
            builder.Entity<Person2Group>()
               .Property(p => p.InsertDate)
               .HasDefaultValueSql("datetime('now')");

            builder.Entity<Person2Group>()
                .Property(p => p.UpdateDate)
                .HasDefaultValueSql("datetime('now')");

            builder.Entity<Person2Group>()
                .Property(p => p.Active)
                .HasDefaultValue(true);

            /// GroupType Enum as string
            var converter = new EnumToStringConverter<GroupType>();

            builder.Entity<Person2Group>()
                .Property(p => p.GroupType)
                .HasConversion(converter);

            /// Person 2 Person2Group
            builder.Entity<Person2Group>()
                .HasOne(g => g.Person)
                .WithMany(p => p.Groups)
                .IsRequired();

            /* TODO разобраться почему не работает
             * builder.Entity<Person2Group>()
                 .HasIndex(g => new { g.Person, g.Active })
                 .IsUnique();*/
        }
        private void InitOrganizationStructureItem(ModelBuilder builder)
        {
            /// Base fields
            builder.Entity<OrganizationStructureItem>()
                .Property(e => e.InsertDate)
                .HasDefaultValueSql("datetime('now')");

            builder.Entity<OrganizationStructureItem>()
                .Property(e => e.UpdateDate)
                .HasDefaultValueSql("datetime('now')");

            builder.Entity<OrganizationStructureItem>()
                .Property(e => e.Active)
                .HasDefaultValue(true);

            /// MaterializedPath
            ValueConverter splitStringConverter = new ValueConverter<ICollection<string>, string>(
                    v => string.Join(";", v),
                    v => v.Split(new[] { ';' })
                );

            builder.Entity<OrganizationStructureItem>()
                .Property(nameof(OrganizationStructureItem.MaterializedPath))
                .HasConversion(splitStringConverter);

            /// OrganizationStructureItem hierarchy
            builder.Entity<OrganizationStructureItem>(entity =>
            {
                entity
                    .HasMany(e => e.Children)
                    .WithOne(e => e.Parent)
                    .HasForeignKey(e => e.ParentId);
            });
        }

        public void InitConfigs(ModelBuilder builder)
        {
            /// Base fields
            builder.Entity<Configuration>()
                .Property(e => e.InsertDate)
                .HasDefaultValueSql("datetime('now')");

            builder.Entity<Configuration>()
                .Property(e => e.UpdateDate)
                .HasDefaultValueSql("datetime('now')");

            builder.Entity<Configuration>()
                .Property(e => e.Active)
                .HasDefaultValue(true);
        }
    }
}
