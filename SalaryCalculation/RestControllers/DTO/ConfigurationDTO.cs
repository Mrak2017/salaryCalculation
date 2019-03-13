using SalaryCalculation.Models;

namespace SalaryCalculation.RestControllers.DTO
{
    public class ConfigurationDTO
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public ConfigurationDTO()
        {
        }

        public ConfigurationDTO(Configuration config)
        {
            Id = config.ID;
            Code = config.Code;
            Value = config.Value;
            Description = config.Decription;
        }
    }
}
