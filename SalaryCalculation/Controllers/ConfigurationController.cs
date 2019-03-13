using SalaryCalculation.Models;
using System;
using System.Globalization;
using System.Linq;

namespace SalaryCalculation.Controllers
{
    public class ConfigurationController
    {
        public const string LAST_DATA_REVISION_CODE = "LastDataRevision";
        public const string BASE_SALARY_POSTFIX = "BaseSalary";
        public const string WORK_EXPERIENCE_RATIO_POSTFIX = "WorkExperienceRatio";
        public const string WORK_EXPERIENCE_MAX_RATIO_POSTFIX = "WorkExperienceMaxRatio";
        public const string SUBORDINATE_RATIO_POSTFIX = "SubordinateRatio";

        private readonly SalaryCalculationDBContext dbContext;

        public ConfigurationController(SalaryCalculationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Configuration[] GetAllConfigs()
        {
            return dbContext.Configs.OrderBy(e => e.ID).ToArray();
        }

        public decimal GetSettingDecimalOrDefault(string code, decimal defaultVal)
        {
            Configuration conf = GetConfigValueByCode(code);
            if (conf != null)
            {
                return decimal.Parse(conf.Value, CultureInfo.InvariantCulture.NumberFormat);
            }

            return defaultVal;
        }

        public int GetSettingIntOrDefault(string code, int defaultVal)
        {
            Configuration conf = GetConfigValueByCode(code);
            if (conf != null)
            {
                return int.Parse(conf.Value, CultureInfo.InvariantCulture.NumberFormat);
            }

            return defaultVal;
        }

        public void AddOrUpdateSetting(string code, string value, string description = "")
        {
            Configuration config = new Configuration
            {
                Code = code,
                Value = value,
                Decription = description
            };

            dbContext.AddOrModify(config, "Code");
        }
        
        private Configuration GetConfigValueByCode(string code)
        {
            return dbContext.Configs
                .Where(c => c.Code.Equals(code))
                .SingleOrDefault();
        }

    }
}
