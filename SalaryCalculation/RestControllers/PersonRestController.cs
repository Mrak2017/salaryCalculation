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
        private readonly PersonController personController;

        public PersonRestController(SalaryCalculationDBContext dbContext)
        {
            personController = new PersonController(dbContext);
        }

        [HttpGet("[action]")]
        public PersonJournalDTO[] AllPersons()
        {
            return this.personController.GetAllPersons()
                .Select(person => this.PreparePersonDTO(person)).ToArray();
        }

        private PersonJournalDTO PreparePersonDTO(Person person)
        {
            GroupType? group = personController.GetPersonGroupOnDate(person, DateTime.Today);
            return new PersonJournalDTO(person, group);
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

            this.personController.AddPerson(person, p2g);
        }
    }
}