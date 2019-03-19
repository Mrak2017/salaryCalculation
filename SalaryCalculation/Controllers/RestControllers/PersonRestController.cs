using Microsoft.AspNetCore.Mvc;
using SalaryCalculation.Models;
using SalaryCalculation.RestControllers.DTO;
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
        public PersonJournalDTO[] GetAllPersons()
        {
            return controller.GetAllPersons()
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
            return new PersonDTO(person, groups);
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

        [HttpPut("[action]/{id}")]
        public void UpdateChief(int id, [FromBody] int chiefId)
        {
            Person person = controller.GetPersonById(id);
            Person chief = controller.GetPersonById(chiefId);

            controller.UpdateChief(person, chief);
        }

        private PersonJournalDTO PreparePersonDTO(Person person)
        {
            GroupType? group = controller.GetPersonGroupOnDate(person, DateTime.Today);
            decimal currentSalary = calculator.CalculateSalary(person, DateTime.Today);
            return new PersonJournalDTO(person, group, currentSalary);
        }
    }
}