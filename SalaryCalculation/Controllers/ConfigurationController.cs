﻿using SalaryCalculation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Controllers
{
    public class ConfigurationController
    {
        public const string BASE_SALLARY_POSTFIX = "BaseSallary";
        public const string WORK_EXPERIENCE_RATIO_POSTFIX = "WorkExperienceRatio";
        public const string WORK_EXPERIENCE_MAX_RATIO_POSTFIX = "WorkExperienceMaxRatio";
        public const string SUBORDINATE_RATIO_POSTFIX = "SubordinateRatio";

        private readonly SalaryCalculationDBContext dbContext;

        public ConfigurationController(SalaryCalculationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string GetConfigValueByName(string name)
        {
            return this.dbContext.Configs
                .Where(c => c.Name.Equals(name))
                .Select(c => c.Value)
                .Single();
        }

        public float GetFloatCastedValueByName(string name)
        {
            return float.Parse(GetConfigValueByName(name), CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
