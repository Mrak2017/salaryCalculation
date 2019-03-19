using Microsoft.EntityFrameworkCore;
using SalaryCalculation.Models;
using System;
using System.Linq;

namespace SalaryCalculation.Controllers
{
    /**Класс для работы с сотрудниками CRUD*/
    public class PersonController
    {
        private readonly SalaryCalculationDBContext dbContext;

        public PersonController(SalaryCalculationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Person[] GetFirstLevelSubordinates(Person person)
        {
            return dbContext.OrganizationStructure
                .Where(o => o.MaterializedPath != null && int.Parse(o.MaterializedPath.Last()) == person.ID)
                .Select(e => e.Person)
                .ToArray();
        }

        public Person[] GetAllSubordinates(Person person)
        {
            return dbContext.OrganizationStructure
                .Where(o => o.MaterializedPath.Contains(Convert.ToString(person.ID)))
                .Select(o => o.Person)
                .ToArray();
        }

        public Person[] GetPossibleChiefs()
        {
            /** выборка с использованием Join*/
            GroupType[] groups = { GroupType.Manager, GroupType.Salesman };
            return dbContext.Persons
                .Join(dbContext.Person2Groups,
                    p => p,
                    g => g.Person,
                    (p, g) => new { Person = p, Person2Group = g })
                .Where(obj =>
                    obj.Person2Group.PeriodStart <= DateTime.Today
                    && (obj.Person2Group.PeriodEnd == null || obj.Person2Group.PeriodEnd >= DateTime.Today)
                    && obj.Person2Group.Active == true
                    && obj.Person.Active == true
                    && Array.IndexOf(groups, obj.Person2Group.GroupType) > -1)
                .Select(obj => obj.Person)
                .Distinct()
                .ToArray();

            /**
             альтернативная более простая выборка
             
            return dbContext.Person2Groups.Where(g =>
                g.PeriodStart <= DateTime.Today
                && (g.PeriodEnd == null || g.PeriodEnd >= DateTime.Today)
                && g.Active == true
                && g.Person.Active == true
                && Array.IndexOf(groups, obj.Person2Group.GroupType) > -1)
                .Select(g => g.Person)
                .Distinct()
                .ToArray();
            */
        }

        public GroupType? GetPersonGroupOnDate(Person person, DateTime onDate)
        {
            Person2Group p2g = dbContext.Person2Groups
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
            return dbContext.Persons.ToArray();
        }

        public void AddPerson(Person person, Person2Group p2g)
        {
            Person existed = FindPersonByLogin(person.Login);
            if (existed != null)
            {
                throw new Exception("Сотрудник с логином '" + person.Login + "' уже существует.");
            }

            dbContext.Persons.Add(person);
            
            p2g.Person = person;
            dbContext.Person2Groups.Add(p2g);

            OrganizationStructureItem item = new OrganizationStructureItem
            {
                Person = person,
                PersonId = person.ID
            };
            dbContext.OrganizationStructure.Add(item);

            dbContext.SaveChanges();
        }

        public Person GetPersonById(int id)
        {
            return dbContext.Persons.Where(e => e.ID == id).SingleOrDefault();
        }

        public Person2Group[] GetAllGroups(Person person)
        {
            return dbContext.Person2Groups
                .Where(g => g.Person == person && g.Active == true)
                .OrderByDescending(g => g.ID)
                .ToArray();
        }

        public void UpdatePerson(Person person)
        {
            Person existed = dbContext.Persons
                .Where(e => e.Login == person.Login && e.ID != person.ID).SingleOrDefault();
            if (existed != null)
            {
                throw new Exception("Сотрудник с логином '" + existed.Login + "' уже существует (id:" + existed.ID + ")");
            }

            dbContext.Entry(person).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void UpdateChief(Person person, Person chief)
        {
            OrganizationStructureItem personItem = GetStructureItem(person);
            OrganizationStructureItem chiefItem = GetStructureItem(chief);

            personItem.Parent = chiefItem;
            personItem.ParentId = chiefItem.ID;

            dbContext.OrganizationStructure.Update(personItem);
            dbContext.SaveChanges();
        }

        public void RecalculateMaterializedPathOrgStructure()
        {
            /// https://github.com/aspnet/EntityFrameworkCore/issues/3241#issuecomment-411928305
        }

        private Person FindPersonByLogin(string login)
        {
            return dbContext.Persons
                .Where(p => p.Login == login)
                .SingleOrDefault();
        }

        private OrganizationStructureItem GetStructureItem(Person person)
        {
            return dbContext.OrganizationStructure.Where(e => e.PersonId == person.ID).SingleOrDefault();
        }
    }
}
