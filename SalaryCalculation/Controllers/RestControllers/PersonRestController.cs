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
        public PersonJournalDTO[] AllPersons()
        {
            return this.controller.GetAllPersons()
                .Select(person => this.PreparePersonDTO(person)).ToArray();
        }

        private PersonJournalDTO PreparePersonDTO(Person person)
        {
            GroupType? group = controller.GetPersonGroupOnDate(person, DateTime.Today);
            decimal currentSalary = calculator.CalculateSalary(person, DateTime.Today);
            return new PersonJournalDTO(person, group, currentSalary);
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

            this.controller.AddPerson(person, p2g);
        }
    }
}