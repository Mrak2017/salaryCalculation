using SalaryCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Controllers
{
    public class SalaryCalculator
    {
        private const string ERROR_START_TEXT = "Ошибка при расчете заработной платы. ";

        private readonly PersonController personController;
        private readonly ConfigurationController configurationController;

        public SalaryCalculator(SalaryCalculationDBContext dbContext)
        {
            this.personController = new PersonController(dbContext);
            this.configurationController = new ConfigurationController(dbContext);
        }

        public decimal CalculateTotalSallary(DateTime onDate)
        {
            Dictionary<Tuple<Person, DateTime>, decimal> calculationCache = new Dictionary<Tuple<Person, DateTime>, decimal>();

            Person[] all = this.personController.GetAllPersons();
            decimal singleSallary = 0;
            foreach (var person in all)
            {
                if (!calculationCache.TryGetValue(new Tuple<Person, DateTime>(person, onDate), out singleSallary))
                {
                    singleSallary = CalculateSallary(person, onDate);
                    calculationCache.Add(new Tuple<Person, DateTime>(person, onDate), singleSallary);
                }
                singleSallary = 0;
            }

            decimal result = 0;
            foreach (KeyValuePair<Tuple<Person, DateTime>, decimal> entry in calculationCache)
            {
                result += entry.Value;
            }
            return result;
        }

        public decimal CalculateSallary(Person person, DateTime onDate)
        {
            GroupType group = this.personController.GetPersonGroupOnDate(person, onDate);
            decimal result = CalculateSallaryBasePart(group, onDate, person);

            switch (group)
            {
                case GroupType.Employee:
                    break;

                case GroupType.Manager:
                    result += CalculateManagerSallaryAddition(person, group, onDate);
                    break;

                case GroupType.Salesman:
                    result += CalculateSalesmanSallaryAddition(person, group, onDate);
                    break;

                default:
                    throw new Exception(ERROR_START_TEXT + "Не удалось определить группу сотрудника: " + person.Login);
            }

            return CheckResult(result, person);
        }

        private decimal CheckResult(decimal value, Person person)
        {
            if (value <= 0)
            {
                throw new Exception(ERROR_START_TEXT + "При расчете зарплата получилась меньше либо равной нулю для сотрудника: "
                            + person.Login + ". Проверьте настройки системы");
            }

            return value;
        }

        private decimal CalculateSallaryBasePart(GroupType group, DateTime onDate, Person person)
        {
            int workedYears = DateUtils.GetFullYearsBetweenDates(person.StartDate, onDate);
            decimal baseSallary = person.BaseSallaryPart.GetValueOrDefault(GetBaseSallaryByGroup(group));
            decimal workExpRatio = GetWorkExperienceRatioByGroup(group);
            decimal workExpMaxRatio = GetWorkExperienceMaxRatioByGroup(group);

            decimal workExpResultRatio = workExpRatio * workedYears;
            if (workExpResultRatio > workExpMaxRatio)
            {
                workExpResultRatio = workExpMaxRatio;
            }
            return baseSallary + (workExpResultRatio * baseSallary);
        }

        private decimal CalculateManagerSallaryAddition(Person person, GroupType group, DateTime onDate)
        {
            decimal subordinateRatio = GetSubordinateRatioByGroup(group);
            Person[] subordinates = this.personController.GetFirstLevelSubordinates(person);
            decimal result = 0;
            foreach (var sub in subordinates)
            {
                result += CalculateSallary(sub, onDate);
            }
            return result * subordinateRatio;
        }

        private decimal CalculateSalesmanSallaryAddition(Person person, GroupType group, DateTime onDate)
        {
            decimal subordinateRatio = GetSubordinateRatioByGroup(group);
            Person[] subordinates = this.personController.GetAllSubordinates(person);
            decimal result = 0;
            foreach (var sub in subordinates)
            {
                result += CalculateSallary(sub, onDate);
            }
            return result * subordinateRatio;
        }

        private decimal GetBaseSallaryByGroup(GroupType group)
        {
            return this.configurationController
                .GetDecimalCastedValueByName(group.ToString() + ConfigurationController.BASE_SALLARY_POSTFIX);
        }

        private decimal GetWorkExperienceRatioByGroup(GroupType group)
        {
            return this.configurationController
                .GetDecimalCastedValueByName(group.ToString() + ConfigurationController.WORK_EXPERIENCE_RATIO_POSTFIX);
        }

        private decimal GetWorkExperienceMaxRatioByGroup(GroupType group)
        {
            return this.configurationController
                .GetDecimalCastedValueByName(group.ToString() + ConfigurationController.WORK_EXPERIENCE_MAX_RATIO_POSTFIX);
        }

        private decimal GetSubordinateRatioByGroup(GroupType group)
        {
            return this.configurationController
                .GetDecimalCastedValueByName(group.ToString() + ConfigurationController.SUBORDINATE_RATIO_POSTFIX);
        }
    }
}
