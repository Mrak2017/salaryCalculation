using SalaryCalculation.Models;
using System;
using System.Collections.Generic;

namespace SalaryCalculation.Controllers
{
    /** Основной класс для расчета заработной платы сотрудников*/
    public class SalaryCalculator
    {
        /** Префикс для текста ошибок*/
        private const string ERROR_START_TEXT = "Ошибка при расчете заработной платы. ";

        private readonly PersonController personController;
        private readonly ConfigurationController configurationController;

        public SalaryCalculator(SalaryCalculationDBContext dbContext)
        {
            personController = new PersonController(dbContext);
            configurationController = new ConfigurationController(dbContext);
        }

        /** Расчет зар платы по всеё фирме, на опредленную дату*/
        public decimal CalculateTotalSalary(DateTime onDate)
        {
            Dictionary<Tuple<Person, DateTime>, decimal> calculationCache = new Dictionary<Tuple<Person, DateTime>, decimal>();

            Person[] all = personController.GetAllPersons();
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

        /** Расчет зар платы по одному сотруднику на определенную дату*/
        public decimal CalculateSalary(Person person, DateTime onDate)
        {
            GroupType? currentGroup = personController.GetPersonGroupOnDate(person, onDate);
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

        /** Проверка результата после расчета зар. платы*/
        private decimal CheckResult(decimal value, Person person)
        {
            if (value < 0)
            {
                throw new Exception(ERROR_START_TEXT + "При расчете зарплата получилась меньше нуля для сотрудника: "
                            + person.Login + ". Проверьте настройки системы.");
            }

            return value;
        }

        /** Расчет базовой ставки сотрудника 
         * (если не заполнена в Person, то дефолтное значение берется из настроек)*/
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

        /** Расчет надбавки для менеджера за непосредственных подчиненных 
         * (процент от зар платы подчиненных 1го уровня, согласно настроек)*/
        private decimal CalculateManagerSalaryAddition(Person person, GroupType group, DateTime onDate)
        {
            decimal subordinateRatio = GetSubordinateRatioByGroup(group);
            Person[] subordinates = personController.GetFirstLevelSubordinates(person);
            decimal result = 0;
            foreach (var sub in subordinates)
            {
                result += CalculateSalary(sub, onDate);
            }
            return result * subordinateRatio;
        }

        /** Расчет надбавки для продажника за всех подчиненных 
         * (процент от зар платы подчиненных всех уровней, согласно настроек)*/
        private decimal CalculateSalesmanSalaryAddition(Person person, GroupType group, DateTime onDate)
        {
            decimal subordinateRatio = GetSubordinateRatioByGroup(group);
            Person[] subordinates = personController.GetAllSubordinates(person);
            decimal result = 0;
            foreach (var sub in subordinates)
            {
                result += CalculateSalary(sub, onDate);
            }
            return result * subordinateRatio;
        }

        /** Получение базовой ставки зар платы из настроек, согласно группы сотрудника*/
        private decimal GetBaseSalaryByGroup(GroupType group)
        {
            return configurationController
                .GetConfigurationDecimalOrDefault(group.ToString() + ConfigurationController.BASE_SALARY_POSTFIX, 0);
        }

        /** Получение коэффициента надбавки за выслугу лет из настроек, по группе сотрудника*/
        private decimal GetWorkExperienceRatioByGroup(GroupType group)
        {
            return configurationController
                .GetConfigurationDecimalOrDefault(group.ToString() + ConfigurationController.WORK_EXPERIENCE_RATIO_POSTFIX, 0) / 100;
        }

        /** Получение максимального коэффициента надбавки за выслугу лет из настроек, по группе сотрудника*/
        private decimal GetWorkExperienceMaxRatioByGroup(GroupType group)
        {
            return configurationController
                .GetConfigurationDecimalOrDefault(group.ToString() + ConfigurationController.WORK_EXPERIENCE_MAX_RATIO_POSTFIX, 0) / 100;
        }

        /** Получение коэффициента надбавки за подчиненных, по группе сотрудника*/
        private decimal GetSubordinateRatioByGroup(GroupType group)
        {
            return configurationController
                .GetConfigurationDecimalOrDefault(group.ToString() + ConfigurationController.SUBORDINATE_RATIO_POSTFIX, 0) / 100;
        }
    }
}
