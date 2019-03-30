using Microsoft.AspNetCore.Mvc;
using SalaryCalculation.Controllers.RestControllers.DTO;
using SalaryCalculation.Models;
using System;
using System.Linq;

namespace SalaryCalculation.Controllers
{
    [Route("api/persons")]
    public class PersonRestController : Controller
    {
        private readonly PersonController controller;
        private readonly SalaryCalculator calculator;

        public PersonRestController(SalaryCalculationDBContext dbContext)
        {
            controller = new PersonController(dbContext);
            calculator = new SalaryCalculator(dbContext);
        }

        [HttpGet("[action]")]
        public PersonJournalDTO[] GetAllPersons(string q = "")
        {
            return controller.GetAllPersons(q)
                .Select(person => PreparePersonDTO(person)).ToArray();
        }
        
        [HttpPost("[action]")]
        public void AddPerson([FromBody] PersonDTO dto)
        {
            Person person = new Person
            {
                Login = dto.Login,
                Password = dto.Password,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                StartDate = dto.StartDate,
                BaseSalaryPart = dto.BaseSalaryPart
            };

            Person2Group p2g = new Person2Group
            {
                PeriodStart = dto.StartDate,
                GroupType = (GroupType)Enum.Parse(typeof(GroupType), dto.CurrentGroup)
            };

            controller.AddPerson(person, p2g);
        }

        [HttpGet("[action]/{id}")]
        public PersonDTO GetPerson(int id)
        {
            Person person = controller.GetPersonById(id);
            if (person == null)
            {
                throw new Exception("Сотрудник с id: " + id + " не найден");
            }

            Person2Group[] groups = controller.GetAllGroups(person);
            Person chief = controller.GetPersonChief(person);
            decimal currentSalary = calculator.CalculateSalary(person, DateTime.Today);
            OrgStructureItemDTO children = GetChildrenOrgStructure(person);
            return new PersonDTO(person, groups, chief, currentSalary, children);
        }

        [HttpPut("[action]/{id}")]
        public void UpdatePerson([FromBody] PersonDTO dto)
        {
            Person person = controller.GetPersonById(dto.Id);
            person.Login = dto.Login;
            person.Password = dto.Password;
            person.FirstName = dto.FirstName;
            person.MiddleName = dto.MiddleName;
            person.LastName = dto.LastName;
            person.StartDate = dto.StartDate;
            person.EndDate = dto.EndDate;
            person.BaseSalaryPart = dto.BaseSalaryPart;

            controller.UpdatePerson(person);
        }

        [HttpGet("[action]")]
        public ComboBoxItemDTO[] GetPossibleChiefs()
        {
            return controller.GetPossibleChiefs()
                .Select(person => new ComboBoxItemDTO(person))
                .ToArray();
        }

        [HttpPut("[action]/{id}/{chiefId}")]
        public void UpdateChief(int id, int chiefId)
        {
            Person person = controller.GetPersonById(id);
            if (chiefId > 0)
            {
                Person chief = controller.GetPersonById(chiefId);
                controller.UpdateChief(person, chief);
            }
            else
            {
                controller.ClearChief(person);
            }            
        }

        [HttpPost("{id}/[action]")]
        public void AddGroup(int id, [FromBody] Person2GroupDTO dto)
        {
            Person person = controller.GetPersonById(id);
            if (person == null)
            {
                throw new Exception("Не удалось найти сотрудника с id '" + id + "'");
            }

            Person2Group p2g = new Person2Group
            {
                PeriodStart = dto.PeriodStart,
                PeriodEnd = dto.PeriodEnd,
                GroupType = (GroupType)Enum.Parse(typeof(GroupType), dto.GroupType)
            };

            controller.AddGroup(person, p2g);
        }

        [HttpGet("[action]/{id}")]
        public Person2GroupDTO GetGroup(int id)
        {
            Person2Group p2g = controller.GetPersonGroupById(id);
            if (p2g == null)
            {
                throw new Exception("Группа с id: " + id + " не найдена");
            }
            return new Person2GroupDTO(p2g);
        }

        [HttpPut("[action]")]
        public void UpdateGroup([FromBody] Person2GroupDTO dto)
        {
            Person2Group p2g = controller.GetPersonGroupById(dto.Id);
            if (p2g == null)
            {
                throw new Exception("Не удалось найти группу сотрудника с id '" + dto.Id + "'");
            }

            p2g.PeriodStart = dto.PeriodStart;
            p2g.PeriodEnd = dto.PeriodEnd;
            p2g.GroupType = (GroupType)Enum.Parse(typeof(GroupType), dto.GroupType);

            controller.UpdateGroup(p2g);
        }

        [HttpDelete("[action]/{id}")]
        public void DeleteGroup(int id)
        {
            Person2Group p2g = controller.GetPersonGroupById(id);
            if (p2g == null)
            {
                throw new Exception("Группа с id:" + id + " не найдена");
            }
            controller.DeleteGroup(p2g);
        }

        [HttpGet("{id}/[action]")]
        public ComboBoxItemDTO[] GetPossibleSubordinates(int id)
        {
            Person person = controller.GetPersonById(id);
            if (person == null)
            {
                throw new Exception("Не удалось найти сотрудника с id '" + id + "'");
            }

            return controller.GetPossibleSubordinates(person)
                .Select(sub => new ComboBoxItemDTO(sub))
                .ToArray();
        }

        
        private PersonJournalDTO PreparePersonDTO(Person person)
        {
            GroupType? group = controller.GetPersonGroupOnDate(person, DateTime.Today);
            decimal currentSalary = calculator.CalculateSalary(person, DateTime.Today);
            return new PersonJournalDTO(person, group, currentSalary);
        }

        private OrgStructureItemDTO GetChildrenOrgStructure(Person person)
        {
            OrgStructureItemDTO[] children = controller.GetFirstLevelSubordinates(person)
                .Select(p => GetChildrenOrgStructure(p))
                .ToArray();

            return new OrgStructureItemDTO(person, children);
        }

    }
}