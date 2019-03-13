using Microsoft.AspNetCore.Mvc;
using SalaryCalculation.Models;
using SalaryCalculation.RestControllers.DTO;
using System;
using System.Linq;

namespace SalaryCalculation.Controllers
{
    [Route("api/configuration")]
    public class ConfigurationRestController : Controller
    {
        private readonly ConfigurationController controller;

        public ConfigurationRestController(SalaryCalculationDBContext dbContext)
        {
            controller = new ConfigurationController(dbContext);
        }

        [HttpGet("[action]")]
        public ConfigurationJournalDTO[] AllConfigs()
        {
            return controller.GetAllConfigs()
                .Select(e => new ConfigurationJournalDTO(e)).ToArray();
        }

        /*
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
        }*/
    }
}