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

        public float CalculateTotalSallary(DateTime onDate)
        {
            Dictionary<Tuple<Person, DateTime>, float> calculationCache = new Dictionary<Tuple<Person, DateTime>, float>();

            Person[] all = this.personController.GetAllPersons();
            float singleSallary = 0;
            foreach (var person in all)
            {
                if (!calculationCache.TryGetValue(new Tuple<Person, DateTime>(person, onDate), out singleSallary))
                {
                    singleSallary = CalculateSallary(person, onDate);
                    calculationCache.Add(new Tuple<Person, DateTime>(person, onDate), singleSallary);
                }
                singleSallary = 0;
            }

            float result = 0;
            foreach (KeyValuePair<Tuple<Person, DateTime>, float> entry in calculationCache)
            {
                result += entry.Value;
            }
            return result;
        }

        public float CalculateSallary(Person person, DateTime onDate)
        {
            GroupType group = this.personController.GetPersonGroupOnDate(person, onDate);
            float result = CalculateSallaryBasePart(group, onDate, person.StartDate);

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

        private float CheckResult(float value, Person person)
        {
            if (value <= 0)
            {
                throw new Exception(ERROR_START_TEXT + "При расчете зарплата получилась меньше либо равной нулю для сотрудника: "
                            + person.Login + ". Проверьте настройки системы");
            }

            return value;
        }

        private float CalculateSallaryBasePart(GroupType group, DateTime onDate, DateTime startDate)
        {
            int workedYears = DateUtils.GetFullYearsBetweenDates(startDate, onDate);
            float baseSallary = GetBaseSallaryByGroup(group);
            float workExpRatio = GetWorkExperienceRatioByGroup(group);
            float workExpMaxRatio = GetWorkExperienceMaxRatioByGroup(group);

            float workExpResultRatio = workExpRatio * workedYears;
            if (workExpResultRatio > workExpMaxRatio)
            {
                workExpResultRatio = workExpMaxRatio;
            }
            return baseSallary + (workExpResultRatio * baseSallary);
        }

        private float CalculateManagerSallaryAddition(Person person, GroupType group, DateTime onDate)
        {
            float subordinateRatio = GetSubordinateRatioByGroup(group);
            Person[] subordinates = this.personController.GetFirstLevelSubordinates(person);
            float result = 0;
            foreach (var sub in subordinates)
            {
                result += CalculateSallary(sub, onDate);
            }
            return result * subordinateRatio;
        }

        private float CalculateSalesmanSallaryAddition(Person person, GroupType group, DateTime onDate)
        {
            float subordinateRatio = GetSubordinateRatioByGroup(group);
            Person[] subordinates = this.personController.GetAllSubordinates(person);
            float result = 0;
            foreach (var sub in subordinates)
            {
                result += CalculateSallary(sub, onDate);
            }
            return result * subordinateRatio;
        }

        private float GetBaseSallaryByGroup(GroupType group)
        {
            // предусмотреть дефолтное значение
            return this.configurationController
                .GetFloatCastedValueByName(group.ToString() + ConfigurationController.BASE_SALLARY_POSTFIX);
        }

        private float GetWorkExperienceRatioByGroup(GroupType group)
        {
            return this.configurationController
                .GetFloatCastedValueByName(group.ToString() + ConfigurationController.WORK_EXPERIENCE_RATIO_POSTFIX);
        }

        private float GetWorkExperienceMaxRatioByGroup(GroupType group)
        {
            return this.configurationController
                .GetFloatCastedValueByName(group.ToString() + ConfigurationController.WORK_EXPERIENCE_MAX_RATIO_POSTFIX);
        }

        private float GetSubordinateRatioByGroup(GroupType group)
        {
            return this.configurationController
                .GetFloatCastedValueByName(group.ToString() + ConfigurationController.SUBORDINATE_RATIO_POSTFIX);
        }
    }
}
