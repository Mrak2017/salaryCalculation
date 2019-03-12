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

        public decimal CalculateTotalSalary(DateTime onDate)
        {
            Dictionary<Tuple<Person, DateTime>, decimal> calculationCache = new Dictionary<Tuple<Person, DateTime>, decimal>();

            Person[] all = this.personController.GetAllPersons();
            decimal singleSalary = 0;
            foreach (var person in all)
            {
                if (!calculationCache.TryGetValue(new Tuple<Person, DateTime>(person, onDate), out singleSalary))
                {
                    singleSalary = CalculateSalary(person, onDate);
                    calculationCache.Add(new Tuple<Person, DateTime>(person, onDate), singleSalary);
                }
                singleSalary = 0;
            }

            decimal result = 0;
            foreach (KeyValuePair<Tuple<Person, DateTime>, decimal> entry in calculationCache)
            {
                result += entry.Value;
            }
            return result;
        }

        public decimal CalculateSalary(Person person, DateTime onDate)
        {
            GroupType? currentGroup = this.personController.GetPersonGroupOnDate(person, onDate);
            if (currentGroup == null)
            {
                return 0;
            }
            GroupType group = currentGroup.GetValueOrDefault();
            decimal result = CalculateSalaryBasePart(group, onDate, person);

            switch (group)
            {
                case GroupType.Employee:
                    break;

                case GroupType.Manager:
                    result += CalculateManagerSalaryAddition(person, group, onDate);
                    break;

                case GroupType.Salesman:
                    result += CalculateSalesmanSalaryAddition(person, group, onDate);
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

        private decimal CalculateSalaryBasePart(GroupType group, DateTime onDate, Person person)
        {
            int workedYears = DateUtils.GetFullYearsBetweenDates(person.StartDate, onDate);
            decimal baseSalary = person.BaseSalaryPart.GetValueOrDefault(GetBaseSalaryByGroup(group));
            decimal workExpRatio = GetWorkExperienceRatioByGroup(group);
            decimal workExpMaxRatio = GetWorkExperienceMaxRatioByGroup(group);

            decimal workExpResultRatio = workExpRatio * workedYears;
            if (workExpResultRatio > workExpMaxRatio)
            {
                workExpResultRatio = workExpMaxRatio;
            }
            return baseSalary + (workExpResultRatio * baseSalary);
        }

        private decimal CalculateManagerSalaryAddition(Person person, GroupType group, DateTime onDate)
        {
            decimal subordinateRatio = GetSubordinateRatioByGroup(group);
            Person[] subordinates = this.personController.GetFirstLevelSubordinates(person);
            decimal result = 0;
            foreach (var sub in subordinates)
            {
                result += CalculateSalary(sub, onDate);
            }
            return result * subordinateRatio;
        }

        private decimal CalculateSalesmanSalaryAddition(Person person, GroupType group, DateTime onDate)
        {
            decimal subordinateRatio = GetSubordinateRatioByGroup(group);
            Person[] subordinates = this.personController.GetAllSubordinates(person);
            decimal result = 0;
            foreach (var sub in subordinates)
            {
                result += CalculateSalary(sub, onDate);
            }
            return result * subordinateRatio;
        }

        private decimal GetBaseSalaryByGroup(GroupType group)
        {
            return this.configurationController
                .GetDecimalCastedValueByName(group.ToString() + ConfigurationController.BASE_SALARY_POSTFIX);
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
