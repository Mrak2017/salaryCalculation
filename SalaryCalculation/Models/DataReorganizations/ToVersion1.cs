using SalaryCalculation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models.DataReorganizations
{
    public class ToVersion1 : ReorganizationMain
    {
        private readonly ConfigurationController controller;

        public ToVersion1(SalaryCalculationDBContext context) : base(context)
        {
            controller = new ConfigurationController(this.context);
        }

        public override void Run()
        {
            InitConfiguration();
        }

        private void InitConfiguration()
        {
            InitBaseSalaryConfig();
            InitWorkExperienceRatioConfig();
            InitWorkExperienceMaxRationConfig();
            InitSubordinateRatio();
        }

        private void InitBaseSalaryConfig()
        {
            controller.AddOrUpdateSetting(
                GroupType.Employee.ToString() + ConfigurationController.BASE_SALARY_POSTFIX,
                100.ToString(),
                "Базовая ставка з/п для группы 'Сотрудник'");

            controller.AddOrUpdateSetting(
                GroupType.Manager.ToString() + ConfigurationController.BASE_SALARY_POSTFIX,
                200.ToString(),
                "Базовая ставка з/п для группы 'Менеджер'");

            controller.AddOrUpdateSetting(
                GroupType.Salesman.ToString() + ConfigurationController.BASE_SALARY_POSTFIX,
                150.ToString(),
                "Базовая ставка з/п для группы 'Продажник'");
        }

        private void InitWorkExperienceRatioConfig()
        {
            controller.AddOrUpdateSetting(
                GroupType.Employee.ToString() + ConfigurationController.WORK_EXPERIENCE_RATIO_POSTFIX,
                3.ToString(),
                "Коэффициент надбавки за время работы для группы 'Сотрудник'");

            controller.AddOrUpdateSetting(
                GroupType.Manager.ToString() + ConfigurationController.WORK_EXPERIENCE_RATIO_POSTFIX,
                5.ToString(),
                "Коэффициент надбавки за время работы для группы 'Менеджер'");

            controller.AddOrUpdateSetting(
                GroupType.Salesman.ToString() + ConfigurationController.WORK_EXPERIENCE_RATIO_POSTFIX,
                1.ToString(),
                "Коэффициент надбавки за время работы для группы 'Продажник'");
        }

        private void InitWorkExperienceMaxRationConfig()
        {
            controller.AddOrUpdateSetting(
                GroupType.Employee.ToString() + ConfigurationController.WORK_EXPERIENCE_MAX_RATIO_POSTFIX,
                30.ToString(),
                "Максимальный коэффициент надбавки за время работы для группы 'Сотрудник'");

            controller.AddOrUpdateSetting(
                GroupType.Manager.ToString() + ConfigurationController.WORK_EXPERIENCE_MAX_RATIO_POSTFIX,
                40.ToString(),
                "Максимальный коэффициент надбавки за время работы для группы 'Менеджер'");

            controller.AddOrUpdateSetting(
                GroupType.Salesman.ToString() + ConfigurationController.WORK_EXPERIENCE_MAX_RATIO_POSTFIX,
                35.ToString(),
                "Максимальный коэффициент надбавки за время работы для группы 'Продажник'");
        }

        private void InitSubordinateRatio()
        {
            controller.AddOrUpdateSetting(
                GroupType.Employee.ToString() + ConfigurationController.SUBORDINATE_RATIO_POSTFIX,
                0.ToString(),
                "Коэффициент надбавки за подчиненных для группы 'Сотрудник'");

            controller.AddOrUpdateSetting(
                GroupType.Manager.ToString() + ConfigurationController.SUBORDINATE_RATIO_POSTFIX,
                0.5.ToString(),
                "Коэффициент надбавки за подчиненных для группы 'Менеджер'");

            controller.AddOrUpdateSetting(
                GroupType.Salesman.ToString() + ConfigurationController.SUBORDINATE_RATIO_POSTFIX,
                0.3.ToString(),
                "Коэффициент надбавки за подчиненных для группы 'Продажник'");
        }
    }
}
