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

        [HttpGet("[action]/{id}")]
        public ConfigurationDTO GetConfig(int id)
        {
            Configuration config = controller.GetConfigurationById(id);
            if (config == null)
            {
                throw new Exception("Настройка с id: " + id + " не найдена");
            }
            return new ConfigurationDTO(config);
        }

        [HttpPost("[action]")]
        public void AddConfig([FromBody] ConfigurationDTO dto)
        {
            controller.AddConfiguration(dto.Code, dto.Value, dto.Description);
        }

        [HttpPut("[action]")]
        public void UpdateConfig([FromBody] ConfigurationDTO dto)
        {
            Configuration conf = controller.GetConfigurationById(dto.Id);
            conf.Code = dto.Code;
            conf.Value = dto.Value;
            conf.Decription = dto.Description;
            controller.UpdateConfiguration(conf);
        }

        [HttpDelete("[action]/{id}")]
        public void DeleteConfig(int id)
        {
            Configuration conf = controller.GetConfigurationById(id);
            if (conf == null)
            {
                throw new Exception("Настройка с id:" + id + " не найдена");
            }
            controller.DeleteConfiguration(conf);
        }
    }
}