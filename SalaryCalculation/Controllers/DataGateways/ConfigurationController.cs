using Microsoft.EntityFrameworkCore;
using SalaryCalculation.Models;
using System;
using System.Globalization;
using System.Linq;

namespace SalaryCalculation.Controllers
{
    /** Класс для работы с системными настройками приложения*/
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

        public decimal GetConfigurationDecimalOrDefault(string code, decimal defaultVal)
        {
            Configuration conf = GetConfigByCode(code);
            if (conf != null)
            {
                return decimal.Parse(conf.Value, CultureInfo.InvariantCulture.NumberFormat);
            }

            return defaultVal;
        }

        public int GetConfigurationIntOrDefault(string code, int defaultVal)
        {
            Configuration conf = GetConfigByCode(code);
            if (conf != null)
            {
                return int.Parse(conf.Value, CultureInfo.InvariantCulture.NumberFormat);
            }

            return defaultVal;
        }

        public void AddOrUpdateConfiguration(string code, string value, string description = "")
        {
            Configuration conf = GetConfigByCode(code);
            if (conf != null)
            {
                conf.Value = value;
                conf.Decription = description;
                dbContext.Entry(conf).State = EntityState.Modified;
            }
            else
            {
                Configuration newConfig = new Configuration
                {
                    Code = code,
                    Value = value,
                    Decription = description
                };
                dbContext.Entry(newConfig).State = EntityState.Added;
            }

            dbContext.SaveChanges();
        }

        public void AddConfiguration(string code, string value, string description = "")
        {
            if (GetConfigByCode(code) != null)
            {
                throw new Exception("Настройка с кодом '" + code + "' уже существует");
            }

            Configuration config = new Configuration
            {
                Code = code,
                Value = value,
                Decription = description
            };

            dbContext.Add(config);
            dbContext.SaveChanges();
        }

        public Configuration GetConfigurationById(int id)
        {
            return dbContext.Configs.Where(e => e.ID == id).SingleOrDefault();
        }

        public void UpdateConfiguration(Configuration configuration)
        {
            Configuration existed = dbContext.Configs
                .Where(e => e.Code == configuration.Code && e.ID != configuration.ID).SingleOrDefault();
            if (existed != null)
            {
                throw new Exception("Настройка с кодом '" + existed.Code + "' уже существует (id:" + existed.ID + ")");
            }

            dbContext.Entry(configuration).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void DeleteConfiguration(Configuration conf)
        {
            dbContext.Configs.Remove(conf);
            dbContext.SaveChanges();
        }

        private Configuration GetConfigByCode(string code)
        {
            return dbContext.Configs
                .Where(c => c.Code.Equals(code))
                .SingleOrDefault();
        }
    }
}
