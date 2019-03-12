using SalaryCalculation.Models;
using System;
using System.Linq;

namespace SalaryCalculation.Controllers
{
    public class PersonController
    {
        private readonly SalaryCalculationDBContext dbContext;

        public PersonController(SalaryCalculationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Person[] GetFirstLevelSubordinates(Person person)
        {
            return this.dbContext.OrganizationStructure
                .Where(o => Int32.Parse(o.MaterializedPath.Last()) == person.ID)
                .Select(o => o.Person)
                .ToArray();
        }

        public Person[] GetAllSubordinates(Person person)
        {
            return this.dbContext.OrganizationStructure
                .Where(o => o.MaterializedPath.Contains(Convert.ToString(person.ID)))
                .Select(o => o.Person)
                .ToArray();
        }

        public GroupType? GetPersonGroupOnDate(Person person, DateTime onDate)
        {
            Person2Group p2g = this.dbContext.Person2Groups
                .Where(g =>
                    g.Person == person
                    && g.PeriodStart <= onDate
                    && (g.PeriodEnd == null || g.PeriodEnd >= onDate)
                    && g.Active == true
                )
                .OrderByDescending(g => g.ID)
                .FirstOrDefault();

            if (p2g != null)
            {
                return p2g.GroupType;
            }
            return null;
        }

        public Person[] GetAllPersons()
        {
            return this.dbContext.Persons.ToArray();
        }

        public void AddPerson(Person person, Person2Group p2g)
        {
            this.dbContext.Persons.Add(person);
            this.dbContext.SaveChanges();

            p2g.Person = person;
            this.dbContext.Person2Groups.Add(p2g);
            this.dbContext.SaveChanges();
        }

        public void RecalculateMaterializedPathOrgStructure()
        {
            /// https://github.com/aspnet/EntityFrameworkCore/issues/3241#issuecomment-411928305
        }
    }
}
