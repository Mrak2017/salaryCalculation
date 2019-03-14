using SalaryCalculation.Models;
using System;

namespace SalaryCalculation.RestControllers.DTO
{
    public class ConfigurationJournalDTO
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
        
        public ConfigurationJournalDTO(Configuration config)
        {
            Id = config.ID;
            Code = config.Code;
            Value = config.Value;
            Description = config.Decription;
            InsertDate = config.InsertDate;
            UpdateDate = config.UpdateDate;
        }
    }
}
