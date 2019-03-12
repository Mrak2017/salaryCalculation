using SalaryCalculation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Controllers
{
    public class ConfigurationController
    {
        public const string BASE_SALARY_POSTFIX = "BaseSalary";
        public const string WORK_EXPERIENCE_RATIO_POSTFIX = "WorkExperienceRatio";
        public const string WORK_EXPERIENCE_MAX_RATIO_POSTFIX = "WorkExperienceMaxRatio";
        public const string SUBORDINATE_RATIO_POSTFIX = "SubordinateRatio";

        private readonly SalaryCalculationDBContext dbContext;

        public ConfigurationController(SalaryCalculationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private string GetConfigValueByCode(string code)
        {
            Configuration config = this.dbContext.Configs
                .Where(c => c.Code.Equals(code))
                .SingleOrDefault();

            if (config == null)
            {
                throw new Exception("Не удалось найти настройку с кодом '" + code + "'.");
            }

            return config.Value;
        }

        public decimal GetDecimalCastedValueByCode(string code)
        {
            return decimal.Parse(GetConfigValueByCode(code), CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
