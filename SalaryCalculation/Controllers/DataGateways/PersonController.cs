using Microsoft.EntityFrameworkCore;
using SalaryCalculation.Models;
using System;
using System.Collections.Generic;
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

        /** Получить список непосредственных подчиненных (первого уровня)*/
        public Person[] GetFirstLevelSubordinates(Person person)
        {
            return dbContext.OrganizationStructure
                .Where(o => o.MaterializedPath != null && int.Parse(o.MaterializedPath.Last()) == person.ID)
                .Select(e => e.Person)
                .ToArray();
        }

        /** Получить список всех подчиненных*/
        public Person[] GetAllSubordinates(Person person)
        {
            return dbContext.OrganizationStructure
                .Where(o => o.MaterializedPath != null && o.MaterializedPath.Contains(Convert.ToString(person.ID)))
                .Select(o => o.Person)
                .ToArray();
        }

        /** Получить список сотрудников, доступных для выбора в качестве руководителя*/
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

        /** Получить тип группы сотрудника на определенную дату*/
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

        /** Получить всех сотрудников*/
        public Person[] GetAllPersons(string search = "")
        {
            return dbContext.Persons
                .Where(p => EF.Functions.Like(p.LastName, "%" + search + "%"))
                .ToArray();
        }

        /** Создать нового сотрудника*/
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

        /** Получить сотрудника по ID*/
        public Person GetPersonById(int id)
        {
            return dbContext.Persons.Where(e => e.ID == id).SingleOrDefault();
        }

        /** Получить все группы сотрудника*/
        public Person2Group[] GetAllGroups(Person person)
        {
            return dbContext.Person2Groups
                .Where(g => g.Person == person && g.Active == true)
                .OrderBy(g => g.PeriodStart)
                .ToArray();
        }

        /** Изменить информацию о сотруднике*/
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

        /** Изменить руководителя для сотрудника*/
        public void UpdateChief(Person person, Person chief)
        {
            OrganizationStructureItem personItem = GetStructureItem(person);
            OrganizationStructureItem chiefItem = GetStructureItem(chief);

            CheckChiefBeforeSave(personItem, chiefItem);

            personItem.ParentId = chiefItem.ID;

            personItem.MaterializedPath = new List<string>();

            if (chiefItem.MaterializedPath != null)
            {
                personItem.MaterializedPath = new List<string>(chiefItem.MaterializedPath);
            }
            
            personItem.MaterializedPath.Add(Convert.ToString(chiefItem.Person.ID));

            dbContext.OrganizationStructure.Update(personItem);
            dbContext.SaveChanges();

            RecalculateMaterializedPathForFirstLevelSubordinates(personItem);
        }
        
        /** Убрать руководителя у сотрудника*/
        public void ClearChief(Person person)
        {
            OrganizationStructureItem item = GetStructureItem(person);

            item.ParentId = null;
            item.MaterializedPath = null;
            
            dbContext.OrganizationStructure.Update(item);
            dbContext.SaveChanges();

            RecalculateMaterializedPathForFirstLevelSubordinates(item);
        }

        /** Получить текущего руководителя для сотрудника*/
        public Person GetPersonChief(Person person)
        {
            OrganizationStructureItem item = GetStructureItem(person);
            if (item.Parent != null)
            {
                return GetLoadedStructureItem(item.Parent).Person;
            }

            return null;
        }

        /** Создать новую группу для сотрудника*/
        public void AddGroup(Person person, Person2Group p2g)
        {
            CheckGroupBeforeSave(person, p2g);

            p2g.Person = person;
            dbContext.Person2Groups.Add(p2g);
            
            dbContext.SaveChanges();
        }

        /** Найти группу по id*/
        public Person2Group GetPersonGroupById(int id)
        {
            return dbContext.Person2Groups
                .Where(e => e.ID == id)
                .Include(e => e.Person)
                .SingleOrDefault();
        }

        /** Изменить инфомарцию о группе*/
        public void UpdateGroup(Person2Group p2g)
        {
            CheckGroupBeforeSave(p2g.Person, p2g);

            dbContext.Entry(p2g).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        /** Удалить инфомарцию о группе*/
        public void DeleteGroup(Person2Group p2g)
        {
            dbContext.Person2Groups.Remove(p2g);
            dbContext.SaveChanges();
        }

        /** Получить список возможных подчиненных для сотрудника*/
        public Person[] GetPossibleSubordinates(Person person)
        {
            Person[] allSubordinates = GetAllSubordinates(person);

            return dbContext.Person2Groups.Where(g =>
                g.PeriodStart <= DateTime.Today
                && (g.PeriodEnd == null || g.PeriodEnd >= DateTime.Today)
                && g.Active == true
                && g.Person.Active == true
                && g.Person != person
                && allSubordinates.Contains(g.Person) == false)
                .Select(g => g.Person)
                .Distinct()
                .ToArray();
        }

        /** Пересчитать материализованный путь, для всех записей в таблице орг структуры*/
        public void RecalculateMaterializedPathOrgStructure()
        {
            /// https://github.com/aspnet/EntityFrameworkCore/issues/3241#issuecomment-411928305
        }

        /** Найти сотрудника по логину*/
        private Person FindPersonByLogin(string login)
        {
            return dbContext.Persons
                .Where(p => p.Login == login)
                .SingleOrDefault();
        }

        /** Получает объект орг структуры по сотруднику, с подгруженным значениями Lazy-loaded полей*/
        private OrganizationStructureItem GetStructureItem(Person person)
        {
            OrganizationStructureItem item = dbContext.OrganizationStructure
                .Where(e => e.PersonId == person.ID)
                .SingleOrDefault();
            return GetLoadedStructureItem(item);
        }

        /** Подгружает Lazy-loaded поля в объект орг структуры*/
        private OrganizationStructureItem GetLoadedStructureItem(OrganizationStructureItem item)
        {
            return dbContext.OrganizationStructure
                .Where(e => (item != null) && (e.ID == item.ID))
                .Include(e => e.Parent)
                .Include(e => e.Person)
                .SingleOrDefault();
        }

        /** Проверка валидности изменения руководителя для сотрудника*/
        private void CheckChiefBeforeSave(OrganizationStructureItem personItem, OrganizationStructureItem chiefItem)
        {
            string personLogin = personItem.Person.Login;
            string chiefLogin = chiefItem.Person.Login;

            if (chiefItem.MaterializedPath != null 
                && chiefItem.MaterializedPath.Contains(Convert.ToString(personItem.Person.ID)))
            {
                throw new Exception("Невозможно назначить сотруднику '" + personLogin
                    + "' руководителя '" + chiefLogin
                    + "', т.к. сотрудник указан среди иерархии руководителей, выбранного руководителя");
            }

            GroupType? personCurrentGroup = GetPersonGroupOnDate(personItem.Person, DateTime.Today);
            if (personCurrentGroup == null)
            {
                throw new Exception("Невозможно назначить руководителя для сотрудника без должности: '" + personLogin + "'");
            }

            GroupType? chiefCurrentGroup = GetPersonGroupOnDate(chiefItem.Person, DateTime.Today);
            if (chiefCurrentGroup == null 
                || (!chiefCurrentGroup.Equals(GroupType.Manager) && !chiefCurrentGroup.Equals(GroupType.Salesman)))
            {
                throw new Exception("Невозможно назначить руководителя '" + chiefLogin 
                    + "', т.к. его текущая должность не 'Менеджер' и не 'Продажник'");
            }
        }

        /** Пересчитать материализованный путь, для непосредственных подчиненных (первого уровня)*/
        private void RecalculateMaterializedPathForFirstLevelSubordinates(OrganizationStructureItem chiefItem)
        {
            Person[] firstLevel = dbContext.OrganizationStructure
                .Where(e => (chiefItem != null) && (e.ParentId == chiefItem.PersonId))
                .Include(e => e.Parent)
                .Include(e => e.Person)
                .Select(e => e.Person)
                .ToArray();

            foreach (var sub in firstLevel)
            {
                OrganizationStructureItem item = GetStructureItem(sub);

                item.MaterializedPath = new List<string>();

                if (chiefItem.MaterializedPath != null)
                {
                    item.MaterializedPath = new List<string>(chiefItem.MaterializedPath);
                }

                item.MaterializedPath.Add(Convert.ToString(chiefItem.Person.ID));

                dbContext.OrganizationStructure.Update(item);
                dbContext.SaveChanges();

                RecalculateMaterializedPathForFirstLevelSubordinates(item);
            }
        }
        /** Проверка уникальности группы для сотрудника в один момент времени*/
        private void CheckGroupBeforeSave(Person person, Person2Group p2g)
        {
            Person2Group[] existed = dbContext.Person2Groups
               .Where(e =>
               person == e.Person
               && p2g.ID != e.ID
               && p2g.PeriodEnd >= e.PeriodStart
               && p2g.PeriodStart <= e.PeriodEnd)
               .ToArray();

            if (existed != null && existed.Length > 0)
            {
                string ids = string.Join(",", existed.Select(e => e.ID).ToArray());
                throw new Exception("У сотрудника не может быть больше 1 группы за период c '" + p2g.PeriodStart
                    + "' по '" + p2g.PeriodEnd + "'. Список идентификаторов: '" + ids + "'");
            }

        }
    }
}
