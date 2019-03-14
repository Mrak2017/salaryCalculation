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

        
        [HttpPost("[action]")]
        public void AddConfig([FromBody] ConfigurationDTO dto)
        {
            controller.AddSetting(dto.Code, dto.Value, dto.Description);
        }
    }
}