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
        private readonly SalaryCalculationDBContext dbContext;
        private readonly PersonController personController;

        public PersonRestController(SalaryCalculationDBContext dbContext)
        {
            this.dbContext = dbContext;
            personController = new PersonController(dbContext);
        }

        [HttpGet("[action]")]
        public PersonJournalDTO[] AllPersons()
        {
            return this.dbContext.Persons.ToArray().Select(person => this.PreparePersonDTO(person)).ToArray();
        }

        private PersonJournalDTO PreparePersonDTO(Person person)
        {
            GroupType group = personController.GetPersonGroupOnDate(person, DateTime.Today);
            return new PersonJournalDTO(person, group);
        }
    }
}